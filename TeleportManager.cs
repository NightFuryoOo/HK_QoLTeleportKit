using HutongGames.PlayMaker;
using Modding;
using System.Collections;
using UnityEngine;

namespace QoLTeleportKit
{
    public class TeleportManager
    {
        private readonly QoLTeleportKit _mod;
        private bool _isBusy;

        public TeleportManager(QoLTeleportKit mod)
        {
            _mod = mod;
            ModHooks.BeforeSceneLoadHook += OnSceneChange;
        }

        public bool IsBusy => _isBusy;

        public void StartTeleport(Vector3 position, string scene)
        {
            if (_isBusy || GameManager.instance.IsGamePaused()) return;

            _isBusy = true;
            _mod.Input.ShowMenu = false;
            _mod.Log.Write($"Starting teleport to {position} in {scene}");
            GameManager.instance.StartCoroutine(TeleportToBoss(position, scene));
        }

        private IEnumerator TeleportToBoss(Vector3 targetPos, string scene)
        {
            if (GameManager.instance.IsGamePaused())
            {
                _isBusy = false;
                _mod.Log.Write("Teleport aborted - game is paused");
                yield break;
            }

            _mod.Log.Write($"Beginning teleport process to {targetPos} in {scene}");

            if (scene == "GG_Workshop" && !PlayerData.instance.GetBool("godseekerUnlocked"))
            {
                PlayerData.instance.SetBool("godseekerUnlocked", true);
                PlayerData.instance.SetBool("godseekerMet", true);
                _mod.Log.Write("Unlocked Godhome access");
            }

            bool isSameScene = GameManager.instance.sceneName == scene;
            bool isDreamRoom = scene == "Dream_Room_Believer_Shrine";
            bool isPOP45 = scene == "White_Palace_18" && targetPos == new Vector3(266.1593f, 12.40812f, 0f);
            bool isPOP46 = scene == "White_Palace_18" && targetPos == new Vector3(162.1888f, 11.40812f, 0f);
            bool isPOP47 = scene == "White_Palace_18" && targetPos == new Vector3(78.96233f, 35.40812f, 0f);
            bool isPOP48 = scene == "White_Palace_17" && targetPos == new Vector3(60.47843f, 33.40812f, 0f);
            bool isPOP49 = scene == "White_Palace_17" && targetPos == new Vector3(32.01745f, 33.40812f, 0f);
            bool isPOP50 = scene == "White_Palace_17" && targetPos == new Vector3(12.39705f, 57.40812f, 0f);
            bool isPOP51 = scene == "White_Palace_17" && targetPos == new Vector3(51.30534f, 63.40812f, 0f);
            bool isPOP52 = scene == "White_Palace_19" && targetPos == new Vector3(88.826f, 33.40812f, 0f);
            bool isPOP53 = scene == "White_Palace_19" && targetPos == new Vector3(153.3704f, 119.4081f, 0f);
            bool isPOP54 = scene == "White_Palace_19" && targetPos == new Vector3(15.4637f, 157.4081f, 0f);
            bool isPOP55 = scene == "White_Palace_20" && targetPos == new Vector3(19.5625f, 169.4081f, 0f);
            bool isPOP56 = scene == "White_Palace_20" && targetPos == new Vector3(228.5202f, 168.5394f, 0f);
            bool isPantheonI = (scene == "GG_Atrium") && targetPos == new Vector3(97.15343f, 35.40812f, 0f);
            bool isPantheonII = (scene == "GG_Atrium") && targetPos == new Vector3(108.4116f, 35.40812f, 0f);
            bool isPantheonIII = (scene == "GG_Atrium") && targetPos == new Vector3(120.2336f, 35.40812f, 0f);
            bool isPantheonIV = (scene == "GG_Atrium") && targetPos == new Vector3(147.3174f, 35.40812f, 0f);
            bool isPantheonV = (scene == "GG_Atrium") && targetPos == new Vector3(132f, 94.6236f, 0f);
            bool isSegmentedPantheonV = (scene == "GG_Atrium_Roof") && targetPos == new Vector3(53.81337f, 19.40812f, 0f);
            bool isPantheonBench = (scene == "GG_Atrium_Roof") && targetPos == new Vector3(120.97f, 42.40812f, 0f);

            if (!isSameScene)
            {
                HeroController.instance.StopAnimationControl();
                HeroController.instance.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                HeroController.instance.RegainControl();

                string targetScene = scene;
                Vector3 entryPos = targetPos;

                if (isDreamRoom)
                {
                    targetScene = "Dream_Room_Believer_Shrine";
                    entryPos = targetPos;
                }
                else if (isPOP48 || isPOP49 || isPOP50 || isPOP51)
                {
                    if (GameManager.instance.sceneName != "White_Palace_17")
                    {
                        targetScene = "White_Palace_19";
                        entryPos = new Vector3(84.16084f, 33.40812f, 0f);
                    }
                }
                else if (isPOP55 || isPOP56)
                {
                    if (GameManager.instance.sceneName != "White_Palace_20")
                    {
                        targetScene = "White_Palace_19";
                        entryPos = new Vector3(15.83391f, 168.5081f, 0f);
                    }
                }
                else if (isPOP52 || isPOP53 || isPOP54)
                {
                    targetScene = "White_Palace_19";
                    entryPos = targetPos;
                }
                else if (isPOP45 || isPOP46 || isPOP47)
                {
                    if (GameManager.instance.sceneName != "White_Palace_18")
                    {
                        targetScene = "White_Palace_06";
                        entryPos = new Vector3(-0.4648393f, 13.73293f, 0f);
                    }
                }
                else if (isPantheonI || isPantheonII || isPantheonIII || isPantheonIV)
                {
                    targetScene = "GG_Workshop";
                    entryPos = new Vector3(-0.4938369f, 6.408124f, 0f);
                }
                else if (scene.StartsWith("White_Palace"))
                {
                    targetScene = "White_Palace_06";
                    entryPos = new Vector3(0.1111f, 7.408124f, 0f);
                }

                _mod.Log.Write($"Loading target scene: {targetScene}");
                var loadInfo = new GameManager.SceneLoadInfo
                {
                    SceneName = targetScene,
                    EntryGateName = "left1",
                    EntryDelay = 0f,
                    WaitForSceneTransitionCameraFade = false,
                    Visualization = GameManager.SceneLoadVisualizations.Default,
                    PreventCameraFadeOut = targetScene.StartsWith("White_Palace") || isDreamRoom
                };

                GameManager.instance.BeginSceneTransition(loadInfo);
                yield return new WaitWhile(() => GameManager.instance.IsInSceneTransition);

                if (targetScene.StartsWith("White_Palace") || isDreamRoom)
                {
                    yield return new WaitForSeconds(0.2f);
                    if (GameManager.instance.cameraCtrl != null)
                    {
                        GameManager.instance.cameraCtrl.FadeSceneIn();
                    }
                }

                HeroController.instance.transform.position = entryPos;
                HeroController.instance.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                if (isDreamRoom)
                {
                    yield return new WaitForSeconds(0.5f);
                    HeroController.instance.transform.position = targetPos;
                    HeroController.instance.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
                else if (isPantheonI || isPantheonII || isPantheonIII || isPantheonIV)
                {
                    yield return new WaitForSeconds(2.5f);

                    HeroController.instance.transform.position = targetPos;
                    HeroController.instance.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                    GameManager.instance.sceneName = "GG_Atrium";
                    if (GameManager.instance.cameraCtrl != null)
                    {
                        GameManager.instance.cameraCtrl.FadeSceneIn();
                    }
                }
                if (isPOP48 || isPOP49 || isPOP50 || isPOP51)
                {
                    if (GameManager.instance.sceneName != "White_Palace_17")
                    {
                        yield return new WaitForSeconds(2.3f);
                        HeroController.instance.transform.position = targetPos;
                        HeroController.instance.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    }
                }
                else if (isPOP45 || isPOP46 || isPOP47)
                {
                    if (GameManager.instance.sceneName != "White_Palace_18")
                    {
                        yield return new WaitForSeconds(2.5f);
                        HeroController.instance.transform.position = targetPos;
                        HeroController.instance.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    }
                }
                else if (isPOP55 || isPOP56)
                {
                    if (GameManager.instance.sceneName != "White_Palace_20")
                    {
                        yield return new WaitForSeconds(2.5f);
                        HeroController.instance.transform.position = targetPos;
                        HeroController.instance.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    }
                }
            }
            else
            {
                HeroController.instance.transform.position = targetPos;
                HeroController.instance.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }

            if (!isSameScene && (scene.StartsWith("White_Palace") || isDreamRoom))
            {
                yield return new WaitForEndOfFrame();
                if (GameManager.instance.cameraCtrl != null)
                {
                    GameManager.instance.cameraCtrl.FadeSceneIn();
                }
            }

            _mod.Log.Write("Teleport completed successfully");
            _isBusy = false;
        }

        private string OnSceneChange(string newSceneName)
        {
            if (_mod.Data.CustomTeleportScene != null && newSceneName != _mod.Data.CustomTeleportScene)
            {
                _mod.Log.Write($"Scene changed from {_mod.Data.CustomTeleportScene} to {newSceneName}, clearing custom teleport point");
                _mod.Data.CustomTeleportPosition = null;
                _mod.Data.CustomTeleportScene = null;
            }
            return newSceneName;
        }
    }
}
