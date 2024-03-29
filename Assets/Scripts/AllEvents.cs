﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

#region GameManager Events
public class GameMenuEvent : SDD.Events.Event
{
}
public class GameInitLevelEvent : SDD.Events.Event
{
}
public class GamePlayEvent : SDD.Events.Event
{
}
public class GamePauseEvent : SDD.Events.Event
{
}
public class GameResumeEvent : SDD.Events.Event
{
}
public class GameOverEvent : SDD.Events.Event
{
}
public class GameVictoryEvent : SDD.Events.Event
{
}

public class GameEndLVLEvent : SDD.Events.Event
{
}

public class GameStatisticsChangedEvent : SDD.Events.Event
{
	public int eNbColisLivree { get; set; }
	public int eStock { get; set; }
	public int eNonLivres { get; set; }
}
#endregion

#region MenuManager Events
public class ButtonActivateGOClickedEvent : SDD.Events.Event
{
	public GameObject eGameObject { get; set; }
}

public class ButtonClickedEvent : SDD.Events.Event
{

}

public class NewGameButtonClickedEvent : SDD.Events.Event
{
}

public class HelpButtonClickedEvent : SDD.Events.Event
{
}

public class CreditButtonClickedEvent : SDD.Events.Event
{
}

public class ExitButtonClickedEvent : SDD.Events.Event
{ 
}

public class MainMenuButtonClickedEvent : SDD.Events.Event
{
}
#endregion

#region SpawnerManger Events

public class StartCooldownSpawnEvent : SDD.Events.Event
{
}

public class StopEachTimeSpawnEvent : SDD.Events.Event
{
}

public class StopEachTimeLinearSpawnEvent : SDD.Events.Event
{
}

public class StopTimedSpawnEvent : SDD.Events.Event
{
}

public class SpawnEachTimeEvent : SDD.Events.Event
{
	public float eSpawnTime { get; set; }
}

public class SpawnEachTimeLinearEvent : SDD.Events.Event
{
	public float eSpawnTime { get; set; }
}


public class SpawnedGameObjectToDestroyEvent : SDD.Events.Event
{
	public GameObject eGameObjectToDestroy { get; set; }
}

public class SpawnNbGOEvent : SDD.Events.Event
{
	public int eNbGOToSpawn { get; set; }
}

public class SpawnGameObjectEvent : SDD.Events.Event
{
	public GameObject eGameObjectToSpawn { get; set; }
}

public class SpawnGameObjectsEvent : SDD.Events.Event
{
	public List<GameObject> eGameObjectsToSpawn { get; set; }
}
#endregion


#region Score Event
public class ScoreHasBeenEarnedEvent : SDD.Events.Event
{
	public int eScore;
}
#endregion

#region Level events
public class LevelFinishEvent : SDD.Events.Event
{
}

public class LevelGameOverEvent : SDD.Events.Event
{
}
#endregion

#region Trigger/Collider events
public class ObjectWillGainScoreEvent : SDD.Events.Event
{
	public GameObject eThisGameObject;
	public GameObject eOtherGO;
}
public class ObjectWillGainStockEvent : SDD.Events.Event
{
	public GameObject eThisGameObject;
	public GameObject eOtherGO;
}

#endregion

#region SFX EVENTS
public class PlaySFXEvent : SDD.Events.Event
{
	public AudioSource eAudioSource { get; set; }
	public AudioClip eAudioClip { get; set; }
}

public class StopSFXEvent : SDD.Events.Event
{
	public AudioSource eAudioSource { get; set; }
	public AudioClip eAudioClip { get; set; }
}
#endregion

#region HUD Events
public class ContinueGameEvent : SDD.Events.Event
{
}

public class CameraChangeUIButtonEvent : SDD.Events.Event
{
}
#endregion