using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

namespace CoronaStriker.Core.Utils
{
    /// <summary>
    /// 배경 음악 및 효과음과 같은 음원을 재생하는 클래스.
    /// </summary>
    public sealed class AudioManager : Singleton<AudioManager>
    {
        #region BGM
        /// <summary>
        /// BGM을 재생하는 Audio Source들의 볼륨을 저장하고 꺼낼 때 사용되는 키 값.
        /// </summary>
        private const string bgmVolumeKey = "BGM Volume";

        /// <summary>
        /// BGM을 재생하는 Audio Source들의 기본 볼륨.
        /// </summary>
        private const float defaultBGMVolume = 0.5f;

        /// <summary>
        /// BGM을 재생하는 Audio Source들의 현재 볼륨.
        /// </summary>
        private float bgmVolume
        {
            get 
            { 
                return PlayerPrefs.HasKey(bgmVolumeKey) ? PlayerPrefs.GetFloat(bgmVolumeKey) : defaultBGMVolume; 
            }

            set
            {
                bgmSpeaker.volume= value;
                powerUpSpeaker.volume = value;
                jingleSpeaker.volume = value;

                PlayerPrefs.SetFloat(bgmVolumeKey, value);
            }
        }

        /// <summary>
        /// 스테에지의 배경 음악과 같은 일반 BGM을 재생하는 Audio Source.
        /// </summary>
        [Header("BGM")]
        [Tooltip("일반적인 배경 음악을 재생하는 BGM Looper.")]
        [SerializeField] private AudioSource bgmSpeaker;
        private MusicLoopModule bgmLoopModule;

        /// <summary>
        /// 무적 상태일 때 흘러나오는 BGM을 재생하는 Audio Source.
        /// </summary>
        [Tooltip("무적 상태일 때 흘러 나오는 배경 음악을 재생하는 BGM Looper.")]
        [SerializeField] private AudioSource powerUpSpeaker;
        private MusicLoopModule powerUpLoopModule;

        /// <summary>
        /// 특수 이벤트(추가 라이프, 기록 갱신 등)일 때 흘러나오는 BGM을 재생하는 Audio Source.
        /// </summary>
        [Tooltip("특수 이벤트(추가 라이프, 기록 갱신 등)일 때 흘러 나오는 짧은 음악을 재생하는 BGM Looper.")]
        [SerializeField] private AudioSource jingleSpeaker;

        /// <summary>
        /// BGM 종류를 나타내는 열거형.
        /// </summary>
        public enum BGMState
        {
            None = 0,
            Main = 1,
            PowerUp = 2,
            Jingle = 3,
        }

        /// <summary>
        /// 현재 재생되고 있는 BGM의 종류.
        /// </summary>
        private BGMState currentBGMState;
        #endregion

        #region SFX
        /// <summary>
        /// SFX를 재생하는 Audio Source들의 볼륨을 저장하고 꺼낼 때 사용되는 키 값.
        /// </summary>
        private const string sfxVolumeKey = "SFX Volume";

        /// <summary>
        /// SFX를 재생하는 Audio Source들의 기본 볼륨.
        /// </summary>
        private const float defaultSFXVolume = 0.5f;

        /// <summary>
        /// SFX를 재생하는 Audio Source들의 현재 볼륨.
        /// </summary>
        private float sfxVolume
        {
            get
            {
                return PlayerPrefs.HasKey(sfxVolumeKey) ? PlayerPrefs.GetFloat(sfxVolumeKey) : defaultSFXVolume;
            }

            set
            {
                foreach (var sfxSpeaker in sfxSpeakers)
                    sfxSpeaker.volume = value;

                PlayerPrefs.SetFloat(sfxVolumeKey, value);
            }
        }

        /// <summary>
        /// Audio Manager가 동시에 재생할 수 있는 기본적으로 SFX 개수.
        /// </summary>
        private const int defaultMaxConcurrentSFXCount = 16;

        /// <summary>
        /// Audio Manager가 동시에 재생할 수 있는 SFX 개수.
        /// </summary>
        [Header("SFX")]
        [Tooltip("Audio Manager가 동시에 재생할 수 있는 SFX 개수.")]
        [SerializeField] private int maxConcurrentSFXCount;

        /// <summary>
        /// SFX(효과음)을 재생하는 Audio Source들.
        /// </summary>
        [Tooltip("SFX(효과음)을 재생하는 Audio Source들.")]
        [SerializeField] private List<AudioSource> sfxSpeakers;

        /// <summary>
        /// 최근 SFX(효과음)을 재생한 Audio Source의 인덱스 번호.
        /// </summary>
        private int currentSourcesIndex;
        #endregion

        protected override void Awake()
        {
            base.Awake();

            //base.Awake();

            #region Initialize
            // 볼륨 초기화.
            if (PlayerPrefs.HasKey(bgmVolumeKey)) bgmVolume = defaultBGMVolume;
            if (PlayerPrefs.HasKey(sfxVolumeKey)) sfxVolume = defaultSFXVolume;

            // 볼륨 설정.
            InitializeBGMVolume();
            InitializeSFXVolume();

            // BGM을 무한 재생하는 Music Loop Module들을 초기화.
            InitializeBGMLoopModules();

            // SFX(효과음) 재생을 담당하는 Audio Source들을 생성.
            InitializeSFXSpeakers();
            #endregion
        }

        private void Update()
        {
            
        }

        /// <summary>
        /// 저장된 설정에 따라 BGM 볼륨을 설정합니다. 만약 저장된 설정이 없다면, 기본 볼륨으로 설정합니다.
        /// </summary>
        private void InitializeBGMVolume()
        {
            if (PlayerPrefs.HasKey(bgmVolumeKey) == false)
            {
                bgmVolume = defaultBGMVolume;
            }    
            else
            {
                bgmSpeaker.volume = bgmVolume;
                powerUpSpeaker.volume = bgmVolume;
                jingleSpeaker.volume = bgmVolume;
            }
        }

        /// <summary>
        /// SFX 볼륨을 초기화합니다.
        /// </summary>
        private void InitializeSFXVolume()
        {
            if (PlayerPrefs.HasKey(sfxVolumeKey) == false)
            {
                sfxVolume = defaultSFXVolume;
            }
            else
            {
                foreach (var sfxSpeaker in sfxSpeakers)
                    sfxSpeaker.volume = sfxVolume;
            }
        }

        /// <summary>
        /// BGM을 무한 재생하는 Music Loop Module들을 초기화합니다.
        /// </summary>
        private void InitializeBGMLoopModules()
        {
            if (bgmSpeaker != null)
                bgmLoopModule = bgmSpeaker.GetComponent<MusicLoopModule>() ?? bgmSpeaker.AddComponent<MusicLoopModule>();

            if (powerUpSpeaker != null)
                powerUpLoopModule = powerUpSpeaker.GetComponent<MusicLoopModule>() ?? powerUpSpeaker.AddComponent<MusicLoopModule>();
        }

        /// <summary>
        /// SFX(효과음) 재생을 담당하는 Audio Source들을 생성합니다.
        /// </summary>
        private void InitializeSFXSpeakers()
        {
            this.sfxSpeakers = new List<AudioSource>();

            var sfxSpeakers = transform.Find("SFX Speakers");

            if (sfxSpeakers == null)
            {
                sfxSpeakers = new GameObject("SFX Speakers").transform;

                sfxSpeakers.SetParent(transform);
                sfxSpeakers.SetPositionAndRotation(transform.position, transform.rotation);
            }

            for (int index = 0; index < maxConcurrentSFXCount; ++index)
            {
                var newSFXSpeaker = new GameObject($"SFX Speaker ({index + 1})").AddComponent<AudioSource>();

                newSFXSpeaker.transform.SetParent(sfxSpeakers);
                newSFXSpeaker.volume = sfxVolume;
                newSFXSpeaker.playOnAwake = false;

                this.sfxSpeakers.Add(newSFXSpeaker);
            }
        }

        /// <summary>
        /// 재생되고 있거나, 재생될 BGM의 재생 시점을 조절합니다.
        /// </summary>
        /// <param name="source">재생 중인 Audio Source.</param>
        /// <param name="time">조절할 시간.</param>
        public void SetTime(AudioSource source, float time)
        {
            source.time = time;
        }

        /// <summary>
        /// 재생되고 있거나, 재생될 BGM의 빠르기를 조절합니다.
        /// </summary>
        /// <param name="source">재생 중인 Audio Source.</param>
        /// <param name="pitch">조절할 빠르기.</param>
        public void SetPitch(AudioSource source, float pitch) 
        {
            source.pitch = pitch;
        }

        /// <summary>
        /// BGM을 재생합니다.
        /// </summary>
        /// <param name="source">재생할 Audio Source.</param>
        /// <param name="bgm">재생할 BGM.</param>
        /// <param name="time">BGM의 재생 시작 시점. (기본값 = 0.0f)</param>
        /// <param name="pitch">BGM의 빠르기. (기본값 = 1.0f)</param>
        public void PlayBGM(AudioSource source, BGM bgm, float time = 0.0f, float pitch = 1.0f)
        {
            source.clip = bgm.clip;
            SetTime(source, time);
            SetPitch(source, pitch);

            source.Play();
        }

        /// <summary>
        /// BGM을 재생합니다.
        /// </summary>
        /// <param name="bgm">재생할 BGM.</param>
        public void PlayMainBGM(BGM bgm)
        {
            if (bgmSpeaker != null)
            {
                powerUpSpeaker?.Stop();
                jingleSpeaker?.Stop();

                // 반복 재생되는 곡이라면 Music Loop Module을 초기화합니다.
                if (bgm.isLoop == true && bgmLoopModule != null)
                    bgmLoopModule.SetFrom(bgm);

                PlayBGM(bgmSpeaker, bgm);
            }
        }

        /// <summary>
        /// 두 번째 BGM을 재생합니다.
        /// </summary>
        /// <param name="bgm"></param>
        public void PlayPowerUpBGM(BGM bgm)
        {
            if (powerUpSpeaker != null && powerUpLoopModule != null) 
            {
                bgmSpeaker?.Pause();

                if (bgm.isLoop == true)
                    powerUpLoopModule.SetFrom(bgm);

                PlayBGM(bgmSpeaker, bgm);
            }
        }

        public void PlayThirdBGM(BGM bgm)
        {
            StartCoroutine(PlayThirdBGMCoroutine(bgm));
        }

        public IEnumerator PlayThirdBGMCoroutine(BGM bgm)
        {
            if (jingleSpeaker != null)
            {
                bgmSpeaker.Pause();
                powerUpSpeaker.Pause();

                PlayBGM(jingleSpeaker, bgm);

                yield return new WaitUntil(() => jingleSpeaker.isPlaying == false);

                FadeInBGM();
            }
        }

        /// <summary>
        /// 일시 정지되었던 BGM을 천천히 재생시킵니다.
        /// </summary>
        /// <param name="source">재생할 Audio Source.</param>
        /// <returns></returns>
        public void FadeInBGM()
        {
            StartCoroutine(FadeInBGMCoroutine(bgmSpeaker));
        }

        /// <summary>
        /// 일시 정지되었던 BGM을 천천히 재생시키는 코루틴.
        /// </summary>
        /// <param name="source">재생할 Audio Source.</param>
        /// <returns></returns>
        public IEnumerator FadeInBGMCoroutine(AudioSource source)
        {
            source.volume = 0.0f;
            source.Play();

            while(true)
            {
                source.volume += 0.3f * Time.deltaTime;

                if (source.volume >= bgmVolume)
                {
                    source.volume = bgmVolume;
                    break;
                }

                yield return null;
            }

            yield return null;
        }

        /// <summary>
        /// 현재 재생되는 BGM의 구간에 맞춰 다음 BGM을 재생합니다.
        /// </summary>
        /// <param name="nextBGM">다음에 재생될 BGM.</param>
        public void PlayBGMByCurrentBGMSync(BGM nextBGM)
        {
            if (bgmSpeaker != null && bgmLoopModule != null)
            {
                // 다음 곡의 재생 시작 지점 = (이전 곡의 현재 시점 * 다음 곡의 루프 종료 지점) / 이전 곡의 루프 종료 지점
                var prevStart = bgmSpeaker.time;
                var prevEnd = bgmLoopModule.loopEnd;
                var nextEnd = nextBGM.loopEnd;

                bgmSpeaker.clip = nextBGM.clip;
                bgmSpeaker.time = (prevStart * nextEnd) / prevEnd;
                bgmSpeaker.Play();
            }
        }
    }
}