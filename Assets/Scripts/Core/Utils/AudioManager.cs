using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

namespace CoronaStriker.Core.Utils
{
    /// <summary>
    /// ��� ���� �� ȿ������ ���� ������ ����ϴ� Ŭ����.
    /// </summary>
    public sealed class AudioManager : Singleton<AudioManager>
    {
        #region BGM
        /// <summary>
        /// BGM�� ����ϴ� Audio Source���� ������ �����ϰ� ���� �� ���Ǵ� Ű ��.
        /// </summary>
        private const string bgmVolumeKey = "BGM Volume";

        /// <summary>
        /// BGM�� ����ϴ� Audio Source���� �⺻ ����.
        /// </summary>
        private const float defaultBGMVolume = 0.5f;

        /// <summary>
        /// BGM�� ����ϴ� Audio Source���� ���� ����.
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
        /// ���׿����� ��� ���ǰ� ���� �Ϲ� BGM�� ����ϴ� Audio Source.
        /// </summary>
        [Header("BGM")]
        [Tooltip("�Ϲ����� ��� ������ ����ϴ� BGM Looper.")]
        [SerializeField] private AudioSource bgmSpeaker;
        private MusicLoopModule bgmLoopModule;

        /// <summary>
        /// ���� ������ �� �귯������ BGM�� ����ϴ� Audio Source.
        /// </summary>
        [Tooltip("���� ������ �� �귯 ������ ��� ������ ����ϴ� BGM Looper.")]
        [SerializeField] private AudioSource powerUpSpeaker;
        private MusicLoopModule powerUpLoopModule;

        /// <summary>
        /// Ư�� �̺�Ʈ(�߰� ������, ��� ���� ��)�� �� �귯������ BGM�� ����ϴ� Audio Source.
        /// </summary>
        [Tooltip("Ư�� �̺�Ʈ(�߰� ������, ��� ���� ��)�� �� �귯 ������ ª�� ������ ����ϴ� BGM Looper.")]
        [SerializeField] private AudioSource jingleSpeaker;

        /// <summary>
        /// BGM ������ ��Ÿ���� ������.
        /// </summary>
        public enum BGMState
        {
            None = 0,
            Main = 1,
            PowerUp = 2,
            Jingle = 3,
        }

        /// <summary>
        /// ���� ����ǰ� �ִ� BGM�� ����.
        /// </summary>
        private BGMState currentBGMState;
        #endregion

        #region SFX
        /// <summary>
        /// SFX�� ����ϴ� Audio Source���� ������ �����ϰ� ���� �� ���Ǵ� Ű ��.
        /// </summary>
        private const string sfxVolumeKey = "SFX Volume";

        /// <summary>
        /// SFX�� ����ϴ� Audio Source���� �⺻ ����.
        /// </summary>
        private const float defaultSFXVolume = 0.5f;

        /// <summary>
        /// SFX�� ����ϴ� Audio Source���� ���� ����.
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
        /// Audio Manager�� ���ÿ� ����� �� �ִ� �⺻������ SFX ����.
        /// </summary>
        private const int defaultMaxConcurrentSFXCount = 16;

        /// <summary>
        /// Audio Manager�� ���ÿ� ����� �� �ִ� SFX ����.
        /// </summary>
        [Header("SFX")]
        [Tooltip("Audio Manager�� ���ÿ� ����� �� �ִ� SFX ����.")]
        [SerializeField] private int maxConcurrentSFXCount;

        /// <summary>
        /// SFX(ȿ����)�� ����ϴ� Audio Source��.
        /// </summary>
        [Tooltip("SFX(ȿ����)�� ����ϴ� Audio Source��.")]
        [SerializeField] private List<AudioSource> sfxSpeakers;

        /// <summary>
        /// �ֱ� SFX(ȿ����)�� ����� Audio Source�� �ε��� ��ȣ.
        /// </summary>
        private int currentSourcesIndex;
        #endregion

        protected override void Awake()
        {
            base.Awake();

            //base.Awake();

            #region Initialize
            // ���� �ʱ�ȭ.
            if (PlayerPrefs.HasKey(bgmVolumeKey)) bgmVolume = defaultBGMVolume;
            if (PlayerPrefs.HasKey(sfxVolumeKey)) sfxVolume = defaultSFXVolume;

            // ���� ����.
            InitializeBGMVolume();
            InitializeSFXVolume();

            // BGM�� ���� ����ϴ� Music Loop Module���� �ʱ�ȭ.
            InitializeBGMLoopModules();

            // SFX(ȿ����) ����� ����ϴ� Audio Source���� ����.
            InitializeSFXSpeakers();
            #endregion
        }

        private void Update()
        {
            
        }

        /// <summary>
        /// ����� ������ ���� BGM ������ �����մϴ�. ���� ����� ������ ���ٸ�, �⺻ �������� �����մϴ�.
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
        /// SFX ������ �ʱ�ȭ�մϴ�.
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
        /// BGM�� ���� ����ϴ� Music Loop Module���� �ʱ�ȭ�մϴ�.
        /// </summary>
        private void InitializeBGMLoopModules()
        {
            if (bgmSpeaker != null)
                bgmLoopModule = bgmSpeaker.GetComponent<MusicLoopModule>() ?? bgmSpeaker.AddComponent<MusicLoopModule>();

            if (powerUpSpeaker != null)
                powerUpLoopModule = powerUpSpeaker.GetComponent<MusicLoopModule>() ?? powerUpSpeaker.AddComponent<MusicLoopModule>();
        }

        /// <summary>
        /// SFX(ȿ����) ����� ����ϴ� Audio Source���� �����մϴ�.
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
        /// ����ǰ� �ְų�, ����� BGM�� ��� ������ �����մϴ�.
        /// </summary>
        /// <param name="source">��� ���� Audio Source.</param>
        /// <param name="time">������ �ð�.</param>
        public void SetTime(AudioSource source, float time)
        {
            source.time = time;
        }

        /// <summary>
        /// ����ǰ� �ְų�, ����� BGM�� �����⸦ �����մϴ�.
        /// </summary>
        /// <param name="source">��� ���� Audio Source.</param>
        /// <param name="pitch">������ ������.</param>
        public void SetPitch(AudioSource source, float pitch) 
        {
            source.pitch = pitch;
        }

        /// <summary>
        /// BGM�� ����մϴ�.
        /// </summary>
        /// <param name="source">����� Audio Source.</param>
        /// <param name="bgm">����� BGM.</param>
        /// <param name="time">BGM�� ��� ���� ����. (�⺻�� = 0.0f)</param>
        /// <param name="pitch">BGM�� ������. (�⺻�� = 1.0f)</param>
        public void PlayBGM(AudioSource source, BGM bgm, float time = 0.0f, float pitch = 1.0f)
        {
            source.clip = bgm.clip;
            SetTime(source, time);
            SetPitch(source, pitch);

            source.Play();
        }

        /// <summary>
        /// BGM�� ����մϴ�.
        /// </summary>
        /// <param name="bgm">����� BGM.</param>
        public void PlayMainBGM(BGM bgm)
        {
            if (bgmSpeaker != null)
            {
                powerUpSpeaker?.Stop();
                jingleSpeaker?.Stop();

                // �ݺ� ����Ǵ� ���̶�� Music Loop Module�� �ʱ�ȭ�մϴ�.
                if (bgm.isLoop == true && bgmLoopModule != null)
                    bgmLoopModule.SetFrom(bgm);

                PlayBGM(bgmSpeaker, bgm);
            }
        }

        /// <summary>
        /// �� ��° BGM�� ����մϴ�.
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
        /// �Ͻ� �����Ǿ��� BGM�� õõ�� �����ŵ�ϴ�.
        /// </summary>
        /// <param name="source">����� Audio Source.</param>
        /// <returns></returns>
        public void FadeInBGM()
        {
            StartCoroutine(FadeInBGMCoroutine(bgmSpeaker));
        }

        /// <summary>
        /// �Ͻ� �����Ǿ��� BGM�� õõ�� �����Ű�� �ڷ�ƾ.
        /// </summary>
        /// <param name="source">����� Audio Source.</param>
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
        /// ���� ����Ǵ� BGM�� ������ ���� ���� BGM�� ����մϴ�.
        /// </summary>
        /// <param name="nextBGM">������ ����� BGM.</param>
        public void PlayBGMByCurrentBGMSync(BGM nextBGM)
        {
            if (bgmSpeaker != null && bgmLoopModule != null)
            {
                // ���� ���� ��� ���� ���� = (���� ���� ���� ���� * ���� ���� ���� ���� ����) / ���� ���� ���� ���� ����
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