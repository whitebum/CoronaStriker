﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CoronaStriker.Core.Utils;
using UnityEngine.Events;

namespace CoronaStriker.Level
{
    /// <summary>
    /// 각종 오디오(BGM, SFX 등)을 재생하는 매니저 클래스.
    /// </summary>
    public sealed class AudioManager : Singleton<AudioManager>
    {
        #region BGM Properties
        /// <summary>
        /// BGM 볼륨을 설정할 때 사용되는 키.
        /// </summary>
        private const string bgmVolumeKey = "BGM Volume";

        /// <summary>
        /// BGM의 기본 볼륨.
        /// </summary>
        private const float defalutBGMVolume = 0.3f;

        /// <summary>
        /// 메인 BGM 재생용 Audio Source.
        /// </summary>
        [Header("BGM")]
        [Tooltip("메인 BGM 재생용 Audio Source.")]
        public AudioSource bgmSource;

        /// <summary>
        /// 메인 BGM을 반복 재생시키는 BGM Loop Module.
        /// </summary>
        [Tooltip("징글 재생용 Audio Source.")]
        [HideInInspector] public BGMLoopModule bgmLoopModule;

        /// <summary>
        /// 특수 상황 BGM 재생용 Audio Source.
        /// </summary>
        [Tooltip("특수 상황 BGM 재생용 Audio Source.")]
        public AudioSource specialSource;

        /// <summary>
        /// 특수 상황 BGM을 반복 재생시키는 BGM Loop Module.
        /// </summary>
        [Tooltip("특수 상황 BGM을 반복 재생시키는 BGM Loop Module.")]
        [HideInInspector] public BGMLoopModule specialLoopModule;

        /// <summary>
        /// 징글 재생용 Audio Source.
        /// </summary>
        [Tooltip("징글 재생용 Audio Source.")]
        public AudioSource jingleSource;

        /// <summary>
        /// 현재 설정된 SFX 볼륨.
        /// </summary>
        private float bgmVolume
        {
            get => PlayerPrefs.HasKey(bgmVolumeKey) ? PlayerPrefs.GetFloat(bgmVolumeKey) : defalutBGMVolume;
            set
            {
                if (value > 0.0f)
                {
                    bgmSource.volume = value;
                    specialSource.volume = value;
                    jingleSource.volume = value;

                    PlayerPrefs.SetFloat(bgmVolumeKey, value);
                }
            }
        }
        #endregion
        #region SFX Properties
        /// <summary>
        /// SFX 볼륨을 설정할 때 사용되는 키.
        /// </summary>
        private const string sfxVolumeKey = "SFX Volume";

        /// <summary>
        /// SFX의 기본 볼륨.
        /// </summary>
        private const float defalutSFXVolume = 0.3f;

        /// <summary>
        /// 동시에 재생 가능한 기본 SFX 개수.
        /// </summary>
        private const int defaultMaxConcurrentSFXCount = 16;

        /// <summary>
        /// 동시에 재생 가능한 SFX 개수.
        /// </summary>
        [Header("SFX")]
        [Tooltip("동시에 재생 가능한 SFX 개수.")]
        [SerializeField] private int maxConcurrentSFXCount;

        /// <summary>
        /// SFX 재생용 Audio Source들.
        /// </summary>
        [Tooltip("SFX를 재생하는 Audio Source들.")]
        [SerializeField] private List<AudioSource> sfxSources;

        /// <summary>
        /// 현재 사용되지 않는 SFX 재생용 Audio Source를 반환합니다.
        /// </summary>
        private AudioSource sfxSource
        {
            get
            {
                var source = sfxSources.FirstOrDefault((s) => s.isPlaying == false);

                if (source == null)
                {
                    var prevCount = maxConcurrentSFXCount + 1;
                    sfxSources.Capacity = (maxConcurrentSFXCount += 5);

                    for (int count = prevCount; count <= maxConcurrentSFXCount; ++count)
                        sfxSources.Add(CreateNewSFXSource(count));

                    return sfxSources[prevCount];
                }

                return source;
            }
        }

        /// <summary>
        /// 현재 설정된 SFX 볼륨.
        /// </summary>
        private float sfxVolume
        {
            get => PlayerPrefs.HasKey(sfxVolumeKey) ? PlayerPrefs.GetFloat(sfxVolumeKey) : defalutSFXVolume;
            set
            {
                if (value > 0.0f)
                {
                    foreach (var source in sfxSources)
                        source.volume = value;

                    PlayerPrefs.SetFloat(sfxVolumeKey, value);
                }
            }
        }
        #endregion

        private Coroutine audioCoroutine;

        private enum BGMState
        {
            None,
            BGM,
            Special,
            Jingle,
        }

        private Stack<BGMState> bgmStates;

        protected override void Awake()
        {
            base.Awake();

            bgmVolume = PlayerPrefs.HasKey(bgmVolumeKey) ? PlayerPrefs.GetFloat(bgmVolumeKey) : defalutBGMVolume;

            bgmLoopModule = bgmSource.GetComponent<BGMLoopModule>() ?? bgmSource.gameObject.AddComponent<BGMLoopModule>();
            specialLoopModule = specialSource.GetComponent<BGMLoopModule>() ?? specialSource.gameObject.AddComponent<BGMLoopModule>();

            CreateSFXSources();
        }

        /// <summary>
        /// 새로운 SFX 재생용 Audio Source를 생성합니다.
        /// </summary>
        /// <param name="index">새로운 SFX 재생용 Audio Source의 인덱스 번호.</param>
        /// <returns></returns>
        private AudioSource CreateNewSFXSource(int index)
        {
            var newSource = new GameObject($"SFX Source ({index})").AddComponent<AudioSource>();
            newSource.transform.SetParent(transform);
            newSource.transform.SetPositionAndRotation(transform.position, transform.rotation);
            newSource.playOnAwake = false;
            newSource.spatialBlend = 0.0f;
            newSource.volume = sfxVolume;

            return newSource;
        }

        /// <summary>
        /// SFX 재생용 Audio Source들을 생성합니다.
        /// </summary>
        private void CreateSFXSources()
        {
            if (maxConcurrentSFXCount <= 0) maxConcurrentSFXCount = defaultMaxConcurrentSFXCount;

            sfxVolume = PlayerPrefs.HasKey(sfxVolumeKey) ? PlayerPrefs.GetFloat(sfxVolumeKey) : defalutSFXVolume;
            sfxSources = new List<AudioSource>();
            sfxSources.Capacity = maxConcurrentSFXCount;

            for (int count = 1; count <= maxConcurrentSFXCount; count++)
                sfxSources.Add(CreateNewSFXSource(count));
        }

        /// <summary>
        /// 메인 BGM을 재생합니다.
        /// </summary>
        /// <param name="bgm">재생할 BGM.</param>
        public void PlayBGM(BGMLoopData bgm)
        {
            if (bgmLoopModule != null)
                bgmLoopModule.SetFrom(bgm);

            PlayBGM(bgm.clip);
        }

        /// <summary>
        /// 메인 BGM을 재생합니다.
        /// </summary>
        /// <param name="bgm">재생할 BGM.</param>
        public void PlayBGM(AudioClip bgm)
        {
            if (bgm == null) return;

            specialSource.Stop();
            jingleSource.Stop();

            bgmSource.clip = bgm;
            bgmSource.Play();
        }

        /// <summary>
        /// 메인 BGM을 일시 정지합니다.
        /// </summary>
        public void PauseBGM()
        {
            if (!bgmSource.isPlaying) return;

            bgmSource.Pause();
        }

        /// <summary>
        /// 메인 BGM을 정지합니다.
        /// </summary>
        public void StopBGM()
        {
            if (!bgmSource.isPlaying) return;

            bgmSource.Stop();
        }

        /// <summary>
        /// 특수 상황 BGM을 재생합니다.
        /// </summary>
        /// <param name="bgm">재생할 BGM.</param>
        public void PlaySpecialBGM(BGMLoopData bgm)
        {
            bgmSource.Pause();
            jingleSource.Stop();

            if (specialLoopModule != null)
                specialLoopModule.SetFrom(bgm);

            specialSource.clip = bgm.clip;
            specialSource.Play();
        }

        /// <summary>
        /// 징글 BGM을 재생합니다.
        /// </summary>
        /// <param name="bgm">재생할 BGM.</param>
        public void PlayJingleBGM(BGMLoopData bgm)
        {
            bgmSource.Pause();
            specialSource.Pause();
        }

        /// <summary>
        /// 지정한 위치에서 SFX를 재생합니다.
        /// </summary>
        /// <param name="sfx"></param>
        /// <param name="position"></param>
        public void PlayAudioClipAtPoint(AudioClip sfx, Vector3 position = default)
        {
            var source = sfxSource;
            source.clip = sfx;
            source.transform.position = position;
            source.Play();
        }
    }
}
