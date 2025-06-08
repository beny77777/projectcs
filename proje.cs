using System;
using System.Collections.Generic;
class Person
{
    public string FullName { get; set; } 
    public string Id { get; set; } 
    public string PhoneNumber { get; set; } 
    public string Address { get; set; } 
}

public class Student : Person
{
   
    private string _studentId;
    private Room _assignedRoom;
    private Block _residenceBlock;
    private Dormitory _residenceDormitory;
    private List<Equipment> _personalItems = new();

  
    public string StudentId
    {
        get { return _studentId; }
        set { _studentId = value; }
    }

    
    public Room AssignedRoom
    {
        get { return _assignedRoom; }
        set { _assignedRoom = value; }
    }

    
    public Block ResidenceBlock
    {
        get { return _residenceBlock; }
        set { _residenceBlock = value; }
    }

   
    public Dormitory ResidenceDormitory
    {
        get { return _residenceDormitory; }
        set { _residenceDormitory = value; }
    }

    public List<Equipment> PersonalItems
    {
        get { return _personalItems; }
        set { _personalItems = value; }//حواسم هست که بعد برای برنامه اصلی . تست حواست به مقدار خالی باشد

    }
}
public class Block
{
    private string _blockName;
    public string BlockName
    {
        get { return _blockName; }
        set { _blockName = value; }
    }
    private int _floorCount;
    public int FloorCount
    {
        get { return _floorCount; }
        set { _floorCount = value; }
    }
    private int _roomCount;
    public int RoomCount
    {
        get { return _roomCount; }
        set { _roomCount = value; }
    }
    private BlockManager _manager;
    public BlockManager Manager
    {
        get { return _manager; }
        set { _manager = value; }
    }

    private List<Room> _rooms = new List<Room>();
    public List<Room> Rooms
    {
        get { return _rooms; }
        set { _rooms = value ?? new List<Room>(); }
    }

    private Dormitory _parentDormitory;
    public Dormitory ParentDormitory
    {
        get { return _parentDormitory; }
        set { _parentDormitory = value; }
    }
}
class Program
{
    static void Main()
    {
    }
} 
