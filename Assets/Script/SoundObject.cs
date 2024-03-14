using UnityEngine;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

public class SoundObject : SerializedMonoBehaviour
{
    public AudioSource Sfx;
    public void Play(AudioClip sounds){
        Sfx.clip=sounds;
        Sfx.Play();
        EndCheck().Forget();
    }
    private async UniTask EndCheck(){
        while(Sfx.isPlaying){
            await UniTask.NextFrame();
        }
        transform.parent.GetComponent<SoundPool>().ReturnObj(this);
    }
}
