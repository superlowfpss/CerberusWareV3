using System;
using System.Runtime.CompilerServices;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
[CompilerGenerated]
public sealed class UiRegistrationSystem : EntitySystem
{
	public override void Initialize()
	{
		this._mainMenu = new MainMenu(this._sysMan);
		RenderManager.Instance.RegisterRender(this._mainMenu);
	}
	public override void Shutdown()
	{
		bool flag = this._mainMenu != null;
		if (flag)
		{
			RenderManager.Instance.UnregisterRender(this._mainMenu);
		}
	}
	
	
	[Robust.Shared.IoC.Dependency] private readonly IEntitySystemManager _sysMan = null;
	
	private MainMenu _mainMenu;
}
