using SCG = System.Collections.Generic;

using Godot;
using Godot.Collections;


public class ExperimentComputerSystem : Node
{
	public void SetPuzzleComputersRandomPuzzles()
	{
		ArrayHashMap<byte, object> puzzleIdSet = new ArrayHashMap<byte, object>();
		int index;

		for(byte i = 1; i < PUZZLE_AMOUNT; i++)
			puzzleIdSet.Add(i, null);
			
		for(byte i = 0; i < 3; i++)
		{
			index = this.RandiRange(rng, 0, puzzleIdSet.Count - 1);
			SetRoomPuzzleComputersPuzzle(i, puzzleIdSet.GetKeyAt(index));
			puzzleIdSet.RemoveAt(index);
		}

		puzzleIdSet.Free();
	}
	
	public void SetPuzzleComputerPuzzle(byte roomId, byte computerId)
	{
		short id = GetComputerKey(roomId, computerId);
		Node c;

		if(puzzleComputerMap.TryGetValue(id, out c))
			c.EmitSignal(SignalKey.SET_PUZZLE, CreatePuzzle(0));
	}

	public void SetRoomPuzzleComputersActive(byte roomId, bool active)
	{
		Node c;

		for(byte i = 0; i < 5; i++)
		{
			if(puzzleComputerMap.TryGetValue(GetComputerKey(roomId, i), out c))
				c.EmitSignal(SignalKey.SET_SYSTEM_ACTIVE, active);
		}
	}
	
	public void UpdateAllInformationComputers()
	{
		SCG.IEnumerator<SCG.KeyValuePair<short, Node>> it =
				informationComputerMap.GetEnumerator();

		while(it.MoveNext())
			it.Current.Value.EmitSignal(SignalKey.UPDATE_INFORMATION_PAGE, 0);
	}

	public void UpdateAllExperimentResultComputers(short subjectID,
			uint experimentStartTime, uint experimentEndTime,
			sbyte experimentLevel, ushort hitsTaken)
	{
		ExperimentResultData erd = GetExperimentResultData(subjectID,
				experimentStartTime, experimentEndTime, experimentLevel, hitsTaken);
		SCG.IEnumerator<SCG.KeyValuePair<short, Node>> it =
				experimentResultComputerMap.GetEnumerator();

		while(it.MoveNext())
			it.Current.Value.EmitSignal(SignalKey.UPDATE_EXPERIMENT_DATA_PAGE, erd);
	}

	public void AddPuzzleComputer(Node computer,
			byte roomId, byte computerId)
	{
		puzzleComputerMap.Add(GetComputerKey(roomId, computerId), computer);
	}

	public void AddInformationComputer(Node computer,
			byte roomId, byte computerId)
	{
		informationComputerMap.Add(GetComputerKey(roomId, computerId), computer);
	}

	public void AddExperimentResultComputer(Node computer,
			byte roomId, byte computerId)
	{
		experimentResultComputerMap.Add(GetComputerKey(roomId, computerId), computer);
	}

	private void SetRoomPuzzleComputersPuzzle(byte roomId, byte puzzleId)
	{
		short computerId;
		Node pcn;

		for(byte pcId = 0; pcId < 5; pcId++)
		{
			computerId = GetComputerKey(roomId, pcId);

			if(puzzleComputerMap.TryGetValue(computerId, out pcn))
				pcn.EmitSignal(SignalKey.SET_PUZZLE, CreatePuzzle(puzzleId));
		}
	}

	private Puzzle CreatePuzzle(byte puzzleId)
	{
		if(puzzleId == 0)
			return new AnyDividedBy0Puzzle();
		else if(puzzleId == 1)
			return new TheLawOfTheRectangleV1Puzzle();
		else if(puzzleId == 2)
			return new KeysToTheTreasureV1Puzzle();
		else if(puzzleId == 3)
			return new NotSoHiddenV1Puzzle();
		else if(puzzleId == 4)
			return new KnowledgeGivenByTheBookV1Puzzle();
		else if(puzzleId == 5)
			return new CarvedInTheCoffinsV1Puzzle();

		return null;
	}

	private ExperimentResultData GetExperimentResultData(short subjectID,
			uint experimentStartTime, uint experimentEndTime,
			sbyte experimentLevel, ushort hitsTaken)
	{
		ExperimentResultData erd = new ExperimentResultData();
		erd.subjectID = subjectID;
		erd.experimentTime = (long) (experimentEndTime - experimentStartTime);
		erd.puzzleSolved = (byte) (experimentLevel + 1);
		erd.allPuzzlesSolved = experimentLevel >= 12;
		erd.hitsTaken = hitsTaken;
		erd.CalculateScore();
		return erd;
	}

	private short GetComputerKey(byte roomId, byte objectId)
	{
		return (byte) ((roomId * 100) + objectId);
	}

	// CHECK: Unused method.
	private bool IsExperimentPuzzleComputer(short computerKey)
	{
		return computerKey % 100 < 5;
	}

	private void Initialize()
	{
		rng = new RandomNumberGenerator();
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public Dictionary<short, Node> PuzzleComputerMap
	{
		set
		{
			puzzleComputerMap = value;
		}
	}

	public Dictionary<short, Node> InformationComputerMap
	{
		set
		{
			informationComputerMap = value;
		}
	}

	public Dictionary<short, Node> ExperimentResultComputerMap
	{
		set
		{
			experimentResultComputerMap = value;
		}
	}

	
	private const byte PUZZLE_AMOUNT = 6;
	
	private RandomNumberGenerator rng;

	private Dictionary<short, Node> puzzleComputerMap;
	private Dictionary<short, Node> informationComputerMap;
	private Dictionary<short, Node> experimentResultComputerMap;
}
