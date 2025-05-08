using System;
using System.Collections.Generic;

[Serializable]
public class LevelData {
    public int level;
    public WinCondition winCondition;
    public GridSize gridSize;
    public List<BoxData> boxes;
}

[Serializable]
public class WinCondition {
    public bool destroyAllBoxes;
    public int scoreTarget;
}

[Serializable]
public class GridSize {
    public int rows;
    public int columns;
}

[Serializable]
public class BoxData {
    public string type;
    public int hp;
    public int row;
    public int column;
}