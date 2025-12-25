using System;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using CerberusWareV3.Configuration;
using Content.Client.StatusIcon;
using Content.Client.UserInterface.Systems;
using Content.Shared.Damage;
using Content.Shared.Damage.Components;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.StatusIcon.Components;
using HarmonyLib;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Shared.Enums;
using Robust.Shared.GameObjects;
using Robust.Shared.Graphics;
using Robust.Shared.IoC;
using Robust.Shared.Maths;
using EntityManagerExt = Robust.Shared.GameObjects.EntityManagerExt;

public class HealthBarOverlay : Overlay
{
    [Robust.Shared.IoC.Dependency] private readonly IEntityManager _entityManager;
    private readonly SharedTransformSystem _transformSystem;
    private readonly MobStateSystem _mobStateSystem;
    private readonly MobThresholdSystem _mobThresholdSystem;
    private readonly StatusIconSystem _statusIconSystem;
    private readonly ProgressColorSystem _progressColorSystem;

    public HealthBarOverlay(IEntityManager entity)
    {
        IoCManager.InjectDependencies(this);
        this._entityManager = entity;
        this._transformSystem = this._entityManager.System<SharedTransformSystem>();
        this._mobStateSystem = this._entityManager.System<MobStateSystem>();
        this._mobThresholdSystem = this._entityManager.System<MobThresholdSystem>();
        this._statusIconSystem = this._entityManager.System<StatusIconSystem>();
        this._progressColorSystem = this._entityManager.System<ProgressColorSystem>();
    }

    public override OverlaySpace Space => (OverlaySpace)8;

    protected override void Draw(in OverlayDrawArgs args)
    {
        if (!CerberusConfig.Hud.ShowHealth)
            return;

        DrawingHandleWorld worldHandle = args.WorldHandle;
        IEye eye = args.Viewport.Eye;
        Angle angle = (eye != null) ? eye.Rotation : Angle.Zero;

        EntityQuery<TransformComponent> entityQuery = this._entityManager.GetEntityQuery<TransformComponent>();
        Vector2 scale = new Vector2(1f, 1f);
        Matrix3x2 scaleMatrix = Matrix3Helpers.CreateScale(ref scale);
        Matrix3x2 rotationMatrix = Matrix3Helpers.CreateRotation(-angle);

        var query = this._entityManager.AllEntityQueryEnumerator<MobThresholdsComponent, MobStateComponent, DamageableComponent, SpriteComponent>();

        while (query.MoveNext(out EntityUid entityUid, out var mobThresholdsComponent, out var mobStateComponent, out var damageableComponent, out var spriteComponent))
        {
            if (!entityQuery.TryGetComponent(entityUid, out var transformComponent) || transformComponent.MapID != args.MapId)
                continue;

            if (damageableComponent.DamageContainerID == null)
                continue;

            // Пропускаем мертвых, если нужно (опционально)
            // if (mobStateComponent.CurrentState == MobState.Dead) continue;

            StatusIconComponent statusIconComponent = EntityManagerExt.GetComponentOrNull<StatusIconComponent>(this._entityManager, entityUid);
            Box2 box = (statusIconComponent?.Bounds) ?? spriteComponent.Bounds;

            var progressInfo = this.CalcProgress(entityUid, mobStateComponent, damageableComponent, mobThresholdsComponent);

            if (progressInfo != null)
            {
                var (ratio, inCrit) = progressInfo.Value;

                Vector2 worldPos = this._transformSystem.GetWorldPosition(transformComponent);
                Matrix3x2 translationMatrix = Matrix3Helpers.CreateTranslation(worldPos);
                Matrix3x2 transformMatrix = Matrix3x2.Multiply(scaleMatrix, translationMatrix);
                transformMatrix = Matrix3x2.Multiply(rotationMatrix, transformMatrix);

                worldHandle.SetTransform(ref transformMatrix);

                float height = box.Height * 32f / 2f - 3f;
                float width = box.Width * 32f;
                Vector2 baseOffset = new Vector2(-width / 32f / 2f, height / 32f);

                Color progressColor = this.GetProgressColor(ratio, inCrit);

                float barWidth = width - 8f;
                float filledWidth = barWidth * ratio + 8f;
                
                Box2 bgBox = new Box2(new Vector2(8f, 0f) / 32f, new Vector2(barWidth + 8f, 3f) / 32f);
                bgBox = bgBox.Translated(baseOffset);
                worldHandle.DrawRect(bgBox, Color.Black.WithAlpha(192), true);
                
                Box2 fgBox = new Box2(new Vector2(8f, 0f) / 32f, new Vector2(filledWidth, 3f) / 32f);
                fgBox = fgBox.Translated(baseOffset);
                worldHandle.DrawRect(fgBox, progressColor, true);
                
                Box2 shadowBox = new Box2(new Vector2(8f, 2f) / 32f, new Vector2(filledWidth, 3f) / 32f);
                shadowBox = shadowBox.Translated(baseOffset);
                worldHandle.DrawRect(shadowBox, Color.Black.WithAlpha(128), true);
            }
        }

        Matrix3x2 identity = Matrix3x2.Identity;
        worldHandle.SetTransform(ref identity);
    }

    private (float ratio, bool inCrit)? CalcProgress(EntityUid uid, MobStateComponent comp, DamageableComponent dmg, MobThresholdsComponent thresholds)
    {
        FieldInfo totalDamageField = AccessTools.Field(dmg.GetType(), "TotalDamage");
        object totalDamageObj = totalDamageField?.GetValue(dmg);

        if (totalDamageObj == null)
            return null;

        FixedPoint2 dmgVal = FixedPoint2.FromObject(totalDamageObj);
        
        FieldInfo thresholdField = AccessTools.Field(dmg.GetType(), "HealthBarThreshold");
        object thresholdObj = thresholdField?.GetValue(dmg);
        FixedPoint2? thresholdVal = thresholdObj != null ? FixedPoint2.FromObject(thresholdObj) : null;

        if (this._mobStateSystem.IsAlive(uid, comp) && thresholdVal != null && dmgVal < thresholdVal.Value)
        {
            return null;
        }

        if (this._mobStateSystem.IsAlive(uid, comp))
        {
            FixedPoint2 critThreshold = GetThresholdForState(MobState.Critical, thresholds);
            FixedPoint2 deadThreshold = GetThresholdForState(MobState.Dead, thresholds);
            
            if (critThreshold == FixedPoint2.Zero)
                critThreshold = deadThreshold;

            if (critThreshold == FixedPoint2.Zero)
            {
                return (1f, false);
            }
            else
            {
                float ratio = 1f - dmgVal.ToFloat() / critThreshold.ToFloat();
                return (Math.Clamp(ratio, 0f, 1f), false);
            }
        }
        else if (this._mobStateSystem.IsCritical(uid, comp))
        {
            FixedPoint2 critThreshold = GetThresholdForState(MobState.Critical, thresholds);
            FixedPoint2 deadThreshold = GetThresholdForState(MobState.Dead, thresholds);

            if (critThreshold == FixedPoint2.Zero || deadThreshold == FixedPoint2.Zero || deadThreshold == critThreshold)
            {
                return ((critThreshold != FixedPoint2.Zero && deadThreshold != FixedPoint2.Zero) ? 0f : 1f, true);
            }
            else
            {
                FixedPoint2 range = deadThreshold - critThreshold;
                float ratio = 1f - (dmgVal - critThreshold).ToFloat() / range.ToFloat();
                return (Math.Clamp(ratio, 0f, 1f), true);
            }
        }

        return (0f, true);
    }

    private FixedPoint2 GetThresholdForState(MobState state, MobThresholdsComponent thresholds)
    {
        try
        {
            FieldInfo dictField = AccessTools.Field(typeof(MobThresholdsComponent), "_thresholds");
            if (dictField != null)
            {
                var dict = dictField.GetValue(thresholds) as System.Collections.IDictionary;
                if (dict != null)
                {
                    foreach (System.Collections.DictionaryEntry entry in dict)
                    {
                        if (entry.Value is MobState entryState && entryState == state)
                        {
                            return FixedPoint2.FromObject(entry.Key);
                        }
                    }
                }
            }
        }
        catch { }

        return FixedPoint2.Zero;
    }

    public Color GetProgressColor(float progress, bool crit)
    {
        if (crit)
        {
            progress = 0f;
        }
        return this._progressColorSystem.GetProgressColor(progress);
    }
}