using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Robust.Shared.ContentPack;
[CompilerGenerated]
public class EntryPoint : GameClient
{
	public override async void PreInit()
	{
		Patcher.PatchAll();
	}
	public override void Init()
	{
		RenderManager.Instance.Initialize();
		RenderManager.Instance.RenderEvent += this.LoadFontAtlas;
		RenderManager.Instance.RegisterRender(new IntroOverlay());
		RenderManager.Instance.RegisterRender(new NotificationManager());
	}
	public override void Shutdown()
	{
		this._cancellationTokenSource.Cancel();
		RenderManager.Instance.RenderEvent -= this.LoadFontAtlas;
		RenderManager.Instance.Dispose();
	}
	private bool LoadFontAtlas()
	{
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		string text = "CerberusWareV3.Resources.Font.Font.ttf";
		bool flag2;
		using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(text))
		{
			bool flag = manifestResourceStream == null;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				byte[] array;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					manifestResourceStream.CopyTo(memoryStream);
					array = memoryStream.ToArray();
				}
				FontManager.AddFont("global-micro", array, 12f);
				FontManager.AddFont("global-small", array, 24f);
				FontManager.AddFont("global", array, 32f);
				FontManager.AddFont("global-large", array, 48f);
				flag2 = true;
			}
		}
		return flag2;
	}
	
	private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
}
