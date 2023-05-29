using UnityEngine;
using Modding;
using MagicUI.Core;
using MagicUI.Elements;
using System;

namespace FloorTimeCounter
{
    public class FloorTimeCounter : Mod, ILocalSettings<LocalSettings>
    {
        public bool ToggleButtonInsideMenu => true;

        new public string GetName() => "FloorTimeCounter";
        public override string GetVersion() => "1.0.0.0";
        public static LocalSettings LS;

        public void OnLoadLocal(LocalSettings s) => LS = s;
        public LocalSettings OnSaveLocal() => LS;

        public override void Initialize()
        {
            On.HeroController.SceneInit += HeroController_SceneInit;
            On.HeroController.Update += HeroController_Update;
        }

        public void Unload()
        {
            Log("Unloading FTC");

            On.HeroController.SceneInit += HeroController_SceneInit;
            On.HeroController.Update -= HeroController_Update;
            text.Destroy();
            layout.Destroy();
        }

        double SecondsGrounded => Math.Round(LS.framesGrounded / 60d, 2);

        LayoutRoot layout;
        TextObject text;
        private void HeroController_SceneInit(On.HeroController.orig_SceneInit orig, HeroController self)
        {
            orig(self);

            Log("Creating FTC");
            layout = new LayoutRoot(false, "Floor Time Counter");

            text = new(layout)
            {
                Text = $"Floor Frames: {LS.framesGrounded}\n{SecondsGrounded}",
                Font = UI.TrajanNormal,
                FontSize = 25
            };
            text.GameObject.transform.position += new Vector3(15, -15);
        }

        private void HeroController_Update(On.HeroController.orig_Update orig, HeroController self)
        {
            orig(self);

            if (self.CheckTouchingGround() && self.acceptingInput && !GameManager.instance.isPaused)
            {
                LS.framesGrounded++;
                text.Text = $"Floor Frames: {LS.framesGrounded}\n{SecondsGrounded}";
            }
        }
    }
}
