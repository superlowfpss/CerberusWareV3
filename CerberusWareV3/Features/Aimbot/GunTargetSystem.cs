using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using CerberusWareV3.Configuration;
using Content.Shared.Damage;
using Content.Shared.Damage.Components;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Systems;
using HarmonyLib;
using Robust.Client.Player;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Systems;


[CompilerGenerated]
public sealed class GunTargetSystem : EntitySystem
{
	public EntityUid? GetClosestTargetToPlayer(EntityUid player, Vector2 circleCenter, float circleRadius, bool targetCritical, GunAimbotOverlay overlay)
	{
		List<EntityUid> list = new List<EntityUid>();
		MapId mapId = this._transformSystem.GetMapId(player);
		foreach (EntityUid entityUid in this._entityLookup.GetEntitiesInRange(mapId, circleCenter, circleRadius))
		{
			bool flag = this.IsValidTarget(entityUid, targetCritical, overlay);
			if (flag)
			{
				bool flag2 = this._transformSystem.GetMapId(entityUid) == this._transformSystem.GetMapId(player);
				if (flag2)
				{
					list.Add(entityUid);
				}
			}
		}
		list.Sort(delegate(EntityUid a, EntityUid b)
		{
			bool flag3 = this._prioritySystem.IsPriority(a);
			bool flag4 = this._prioritySystem.IsPriority(b);
			bool flag5 = flag3 && !flag4;
			int num;
			if (flag5)
			{
				num = -1;
			}
			else
			{
				bool flag6 = !flag3 && flag4;
				if (flag6)
				{
					num = 1;
				}
				else
				{
					float num2 = (this._transformSystem.GetWorldPosition(a) - this._transformSystem.GetWorldPosition(player)).Length();
					float num3 = (this._transformSystem.GetWorldPosition(b) - this._transformSystem.GetWorldPosition(player)).Length();
					num = num2.CompareTo(num3);
				}
			}
			return num;
		});
		bool onlyPriority = CerberusConfig.GunAimBot.OnlyPriority;
		EntityUid? entityUid2;
		if (onlyPriority)
		{
			entityUid2 = new EntityUid?(list.FirstOrDefault((EntityUid t) => this._prioritySystem.IsPriority(t)));
		}
		else
		{
			entityUid2 = new EntityUid?(list.FirstOrDefault<EntityUid>());
		}
		return entityUid2;
	}
	public EntityUid? GetClosestTargetToMouse(Vector2 circleCenter, float circleRadius, bool targetCritical, GunAimbotOverlay overlay)
	{
		List<EntityUid> list = new List<EntityUid>();
		EntityUid value = this._playerManager.LocalEntity.Value;
		MapId mapId = this._transformSystem.GetMapId(value);
		foreach (EntityUid entityUid in this._entityLookup.GetEntitiesInRange(mapId, circleCenter, circleRadius))
		{
			bool flag = this.IsValidTarget(entityUid, targetCritical, overlay);
			if (flag)
			{
				float num = (this._transformSystem.GetWorldPosition(entityUid) - circleCenter).LengthSquared();
				bool flag2 = num <= circleRadius * circleRadius;
				if (flag2)
				{
					list.Add(entityUid);
				}
			}
		}
		list.Sort(delegate(EntityUid a, EntityUid b)
		{
			bool flag3 = this._prioritySystem.IsPriority(a);
			bool flag4 = this._prioritySystem.IsPriority(b);
			bool flag5 = flag3 && !flag4;
			int num2;
			if (flag5)
			{
				num2 = -1;
			}
			else
			{
				bool flag6 = !flag3 && flag4;
				if (flag6)
				{
					num2 = 1;
				}
				else
				{
					float num3 = (this._transformSystem.GetWorldPosition(a) - circleCenter).Length();
					float num4 = (this._transformSystem.GetWorldPosition(b) - circleCenter).Length();
					num2 = num3.CompareTo(num4);
				}
			}
			return num2;
		});
		bool onlyPriority = CerberusConfig.GunAimBot.OnlyPriority;
		EntityUid? entityUid2;
		if (onlyPriority)
		{
			entityUid2 = new EntityUid?(list.FirstOrDefault((EntityUid t) => this._prioritySystem.IsPriority(t)));
		}
		else
		{
			entityUid2 = new EntityUid?(list.FirstOrDefault<EntityUid>());
		}
		return entityUid2;
	}
	public EntityUid? GetHighestHealthTarget(Vector2 circleCenter, float circleRadius, bool targetCritical, GunAimbotOverlay overlay)
	{
		List<EntityUid> list = new List<EntityUid>();
		EntityUid value = this._playerManager.LocalEntity.Value;
		MapId mapId = this._transformSystem.GetMapId(value);
		foreach (EntityUid entityUid in this._entityLookup.GetEntitiesInRange(mapId, circleCenter, circleRadius))
		{
			float num = (this._transformSystem.GetWorldPosition(entityUid) - circleCenter).LengthSquared();
			DamageableComponent damageableComponent;
			bool flag = num <= circleRadius * circleRadius && this._entityManager.TryGetComponent<DamageableComponent>(entityUid, out damageableComponent) && this.IsValidTarget(entityUid, targetCritical, overlay);
			if (flag)
			{
				list.Add(entityUid);
			}
		}
		list.Sort(delegate(EntityUid a, EntityUid b)
		{
			bool flag2 = this._prioritySystem.IsPriority(a);
			bool flag3 = this._prioritySystem.IsPriority(b);
			bool flag4 = flag2 && !flag3;
			int num2;
			if (flag4)
			{
				num2 = -1;
			}
			else
			{
				bool flag5 = !flag2 && flag3;
				if (flag5)
				{
					num2 = 1;
				}
				else
				{
					DamageableComponent componentOrNull = EntityManagerExt.GetComponentOrNull<DamageableComponent>(this._entityManager, a);
					DamageableComponent componentOrNull2 = EntityManagerExt.GetComponentOrNull<DamageableComponent>(this._entityManager, b);
					bool flag6 = componentOrNull == null || componentOrNull2 == null;
					if (flag6)
					{
						num2 = 0;
					}
					else
					{
						FieldInfo fieldInfo = AccessTools.Field(componentOrNull.GetType(), "TotalDamage");
						object obj = ((fieldInfo != null) ? fieldInfo.GetValue(componentOrNull) : null);
						FieldInfo fieldInfo2 = AccessTools.Field(componentOrNull2.GetType(), "TotalDamage");
						object obj2 = ((fieldInfo2 != null) ? fieldInfo2.GetValue(componentOrNull2) : null);
						bool flag7 = obj == null || obj2 == null;
						if (flag7)
						{
							num2 = 0;
						}
						else
						{
							FixedPoint2 dmgVal = FixedPoint2.FromObject(obj);
							FixedPoint2 thresholdVal = FixedPoint2.FromObject(obj2);
							int num3 = dmgVal.CompareTo(thresholdVal);
							bool flag8 = num3 != 0;
							if (flag8)
							{
								num2 = num3;
							}
							else
							{
								float num4 = (this._transformSystem.GetWorldPosition(a) - circleCenter).Length();
								float num5 = (this._transformSystem.GetWorldPosition(b) - circleCenter).Length();
								num2 = num4.CompareTo(num5);
							}
						}
					}
				}
			}
			return num2;
		});
		bool onlyPriority = CerberusConfig.GunAimBot.OnlyPriority;
		EntityUid? entityUid2;
		if (onlyPriority)
		{
			entityUid2 = new EntityUid?(list.FirstOrDefault((EntityUid t) => this._prioritySystem.IsPriority(t)));
		}
		else
		{
			entityUid2 = new EntityUid?(list.FirstOrDefault<EntityUid>());
		}
		return entityUid2;
	}
	private bool IsValidTarget(EntityUid entity, bool targetCritical, GunAimbotOverlay overlay)
	{
		MobStateComponent mobStateComponent = null;
		bool flag = entity == this._playerManager.LocalEntity || !this._entityManager.TryGetComponent<MobStateComponent>(entity, out mobStateComponent);
		bool flag2;
		if (flag)
		{
			flag2 = false;
		}
		else
		{
			bool flag3 = this._friendSystem.IsFriend(entity);
			if (flag3)
			{
				flag2 = false;
			}
			else
			{
				bool flag4 = mobStateComponent.CurrentState == (MobState)2 && targetCritical;
				if (flag4)
				{
					foreach (EntityUid entityUid in this._entityLookup.GetEntitiesInRange(this._transformSystem.GetMapId(entity), overlay.MouseWorldPos, CerberusConfig.GunAimBot.CircleRadius))
					{
						bool flag5 = entityUid == this._playerManager.LocalEntity;
						if (!flag5)
						{
							MobStateComponent mobStateComponent2 = null;
							bool flag6 = entityUid == entity || !this._entityManager.TryGetComponent<MobStateComponent>(entityUid, out mobStateComponent2);
							if (!flag6)
							{
								bool flag7 = mobStateComponent2.CurrentState != (MobState)2;
								if (flag7)
								{
									return false;
								}
							}
						}
					}
				}
				MobState currentState = mobStateComponent.CurrentState;
				MobState mobState = currentState;
				if (mobState != (MobState)2)
				{
					if (mobState == (MobState)3)
					{
						return false;
					}
				}
				else if (!targetCritical)
				{
					return false;
				}
				flag2 = this.CanHitTargetWithHitScan(this._playerManager.LocalEntity.Value, entity, null) || (CerberusConfig.GunAimBot.PredictEnabled && overlay.PredictedPos != null && this.CanHitTargetWithHitScan(this._playerManager.LocalEntity.Value, entity, new EntityCoordinates?(overlay.PredictedPos.Value)));
			}
		}
		return flag2;
	}
	public bool CanHitTargetWithHitScan(EntityUid userUid, EntityUid targetUid, EntityCoordinates? targetCoords = null)
	{
		bool flag = !CerberusConfig.GunAimBot.HitScan;
		bool flag2 = false;
		if (flag)
		{
			flag2 = true;
		}
		else
		{
			MapCoordinates mapCoordinates = this._transformSystem.GetMapCoordinates(userUid, null);
			Vector2 vector = ((targetCoords != null) ? this._transformSystem.ToMapCoordinates(targetCoords.Value, true).Position : this._transformSystem.GetMapCoordinates(targetUid, null).Position);
			EntityUid entityUid;
			GunComponent gunComponent;
			bool flag3 = !this._gunSystem.TryGetGun(userUid, out entityUid, out gunComponent);
			if (flag3)
			{
				flag2 = false;
			}
			else
			{
				/*/bool flag4 = this._entityManager.HasComponent<>(entityUid);
				int num;
				if (flag4)
				{
					num = 1;
				}
				else
				{
					num = 64;
				}
				Vector2 vector2 = vector - mapCoordinates.Position;
				CollisionRay collisionRay;
				collisionRay(mapCoordinates.Position, Vector2Helpers.Normalized(vector2), num);
				List<RayCastResults> list = this._physics.IntersectRay(mapCoordinates.MapId, collisionRay, vector2.Length(), new EntityUid?(userUid), true).ToList<RayCastResults>();
				bool flag5 = !list.Any<RayCastResults>();/*/
			}
		}
		return flag2;
	}
	
	[Robust.Shared.IoC.Dependency] private readonly IEntityManager _entityManager = null;
	
	[Robust.Shared.IoC.Dependency] private readonly IPlayerManager _playerManager = null;
	
	[Robust.Shared.IoC.Dependency] private readonly EntityLookupSystem _entityLookup = null;
	
	[Robust.Shared.IoC.Dependency] private readonly PrioritySystem _prioritySystem = null;
	
	[Robust.Shared.IoC.Dependency] private readonly FriendSystem _friendSystem = null;
	
	[Robust.Shared.IoC.Dependency]  private readonly SharedGunSystem _gunSystem = null;
	
	private readonly SharedPhysicsSystem _physics = null;
	
	[Robust.Shared.IoC.Dependency] private readonly SharedTransformSystem _transformSystem = null;
}
