using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for playing different sounds
/// the prefab is added per scene, so it doesn't contain sounds not required in the scene
/// </summary>
public class AudioPlayer : MonoBehaviour
{
    // constants storing names of the sounds that will be used as keys to play stored sounds
    private const string TILE_TAPPED_SFX = "tileTapped";
    private const string INVALID_MOVE_SFX = "invalidMove";
    
    private const string LEVEL_WON_SFX = "levelWon";
    private const string LEVEL_LOST_SFX = "levelLost";
    
    private const string LEVEL_SELECTED_SFX = "levelSelected";
    private const string RANDOM_MODE_SELECTED_SFX = "randomModeSelected";

    // all the sounds in the current scene will be stored here and played as needed
    private Dictionary<string, SoundSource> soundLibrary;
    
    private void Awake()
    {
        GameEvents.ActiveTileTappedEvent -= OnActiveTileTapped;
        GameEvents.ActiveTileTappedEvent += OnActiveTileTapped;
        
        GameEvents.InvalidMoveEvent -= OnInvalidMove;
        GameEvents.InvalidMoveEvent += OnInvalidMove;
        
        GameEvents.LevelEndedEvent -= OnLevelEnded;
        GameEvents.LevelEndedEvent += OnLevelEnded;
        
        UIEvents.LevelSelectedEvent -= OnLevelSelected;
        UIEvents.LevelSelectedEvent += OnLevelSelected;
        
        UIEvents.RandomModeSelectedEvent -= OnRandomModeSelected;
        UIEvents.RandomModeSelectedEvent += OnRandomModeSelected;
    }

    private void Start()
    {
        // populate the sound library with sounds for the scene
        // the sound sources are added as child game objects in different presets of the audio player
        soundLibrary = new Dictionary<string, SoundSource>();
        SoundSource[] soundSources = GetComponentsInChildren<SoundSource>();
        foreach (SoundSource sound in soundSources)
        {
            // the sound name specified in the SoundSource is used as a key, and should match one of the constants above
            soundLibrary[sound.soundName] = sound;
        }
    }
    
    private void OnDestroy()
    {
        GameEvents.ActiveTileTappedEvent -= OnActiveTileTapped;
        GameEvents.InvalidMoveEvent -= OnInvalidMove;
        GameEvents.LevelEndedEvent -= OnLevelEnded;
        UIEvents.LevelSelectedEvent -= OnLevelSelected;
        UIEvents.RandomModeSelectedEvent -= OnRandomModeSelected;
    }
    

    // look up a sound source by the name as key, and play the associated sound
    private void PlaySoundFromName(string soundSourceName)
    {
        if (!soundLibrary.ContainsKey(soundSourceName) || soundLibrary[soundSourceName] == null)
        {
            return;
        }

        soundLibrary[soundSourceName].PlaySound();
    }
    
    // look up a sound source by the name as key, and stop the associated sound
    private void StopSoundFromName(string soundSourceName)
    {
        if (!soundLibrary.ContainsKey(soundSourceName) || soundLibrary[soundSourceName] == null)
        {
            return;
        }

        soundLibrary[soundSourceName].StopSound();
    }
    
    // the following handlers just play the different sounds in response to different events in the session
    
    private void OnActiveTileTapped(int gridY, int gridX)
    {
        PlaySoundFromName(TILE_TAPPED_SFX);
    }
    
    private void OnInvalidMove(int gridY, int gridX)
    {
        StopSoundFromName(TILE_TAPPED_SFX);
        PlaySoundFromName(INVALID_MOVE_SFX);
    }
    
    private void OnLevelEnded(bool won)
    {
        PlaySoundFromName(won ? LEVEL_WON_SFX : LEVEL_LOST_SFX);
    }
    
    private void OnLevelSelected(string levelName)
    {
        PlaySoundFromName(LEVEL_SELECTED_SFX);
    }
    
    private void OnRandomModeSelected()
    {
        PlaySoundFromName(RANDOM_MODE_SELECTED_SFX);
    }
}
