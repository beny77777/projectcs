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

    public Student()
{
    _personalItems = new List<Equipment>();
}

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
    set { _personalItems = value ?? new List<Equipment>(); }
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
//کلاس تجهیزات(کامنت توسط خودمه )
public class Equipment
{
    public enum EquipmentType { Bed, Closet, Table, Chair, Refrigerator }
    public enum EquipmentStatus { Healthy, Damaged, InRepair }

    public EquipmentType Type { get; set; }
    public string PartNumber { get; set; }
    public string AssetNumber { get; set; } 
    public EquipmentStatus Status { get; set; }

    public Room AssignedRoom { get; set; }
    public Student OwnerStudent { get; set; } 
}
//کامنت خودمه:کلاس اتاق ها(برای اینکه بفهمم هربخش برای چیه)
public class Room
{
    public string RoomNumber { get; set; }
    public int Floor { get; set; }
    public int Capacity { get; set; } = 6; // حداکثر 6 نفر

    private List<Equipment> _equipments = new();
    public List<Equipment> Equipments
    {
        get { return _equipments; }
set
{
    if (value != null)
        _equipments = value;
    else
        _equipments = new List<Equipment>();
}
    }

    private List<Student> _residents = new();
    public List<Student> Residents
    {
        get { return _residents; }
       set
{
    if (value != null)
        _residents = value;
    else
        _residents = new List<Student>();
}

    }

    public Block ParentBlock { get; set; }

    public bool IsFull
{
    get
    {
        if (Residents.Count < Capacity)
            return false;
        else
            return true;
    }
}
public int CurrentCapacity()
{
    return Students.Count;
}

public bool IsFull()
{
    return Students.Count >= Capacity;
}

}
// کلاس برای خوابگاه
public class Dormitory
{
    public string Name { get; set; }
    public string Address { get; set; }
    public int TotalCapacity { get; set; }

    private DormitoryManager _manager;
    public DormitoryManager Manager
    {
        get { return _manager; }
        set { _manager = value; }
    }

    private List<Block> _blocks = new();
    public List<Block> Blocks
    {
        get { return _blocks; }
        set
         {
    if (value != null)
        _blocks = value;
    else
        _blocks = new List<Block>();
         }

    }
    public int CurrentCapacity()
{
    int count = 0;
    foreach (var room in Rooms)
    {
        count += room.CurrentCapacity();
    }
    return count;
}

public int RemainingCapacity()
{
    return Capacity - CurrentCapacity();
}

}
// کلاس برای مدیر خوابگاه
public class DormitoryManager : Person
{
    public string Position { get; set; }
    public Dormitory AssignedDormitory { get; set; }
}
//کلاس برای مدیر بلوک
public class BlockManager : Student 
{
    public string Position { get; set; }
    public Block AssignedBlock { get; set; }
}
public class DormitorySystem
{
    private List<Dormitory> _dormitories = new();
    private List<Student> _students = new();
    private List<DormitoryManager> _dormManagers = new();
    private List<BlockManager> _blockManagers = new();
    private List<Equipment> _allEquipments = new();

    public List<Dormitory> Dormitories => _dormitories;
    public List<Student> Students => _students;
    public List<DormitoryManager> DormitoryManagers => _dormManagers;
    public List<BlockManager> BlockManagers => _blockManagers;
    public List<Equipment> Equipments => _allEquipments;
    public void AddDormitory(Dormitory dorm)
{
    if (dorm != null && !_dormitories.Contains(dorm))
        _dormitories.Add(dorm);
}
public void AddStudent(Student student)
{
    if (student != null && !_students.Contains(student))
        _students.Add(student);
}
 public void AddDormitory(Dormitory dormitory)
{
    if (dormitory != null && !_dormitories.Contains(dormitory))
        _dormitories.Add(dormitory);
}
public void AddBlockToDormitory(Block block, Dormitory dormitory)
{
    if (block != null && dormitory != null && dormitories.Contains(dormitory))
    {
        dormitory.Blocks.Add(block);
        block.ParentDormitory = dormitory;
    }
}
public void AddStudent(Student student)
{
    if (!_students.Contains(student))
        _students.Add(student);
}
public void RemoveStudent(Student student)
{
    if (student != null && _students.Contains(student))
    {
        _students.Remove(student);
    }
}
public bool RemoveBlock(string dormName, string blockName)
{
    Dormitory dorm = GetDormitory(dormName);
    if (dorm == null) return false;

    Block block = null;
    foreach (Block b in dorm.Blocks)
    {
        if (b.BlockName == blockName)
        {
            block = b;
            break;
        }
    }

    if (block != null)
    {
        dorm.Blocks.Remove(block);
        return true;
    }
    return false;
}
public bool AddBlockToDormitory(string dormName, Block block)
{
    Dormitory dorm = GetDormitory(dormName);
    if (dorm == null) return false;
    if (block.Manager == null || !students.Contains(block.Manager)) return false;
    block.ParentDormitory = dorm;
    dorm.Blocks.Add(block);
    return true;
}
public Block GetBlock(string dormName, string blockName)
{
    Dormitory dorm = GetDormitory(dormName);
    if (dorm == null) return null;

    foreach (Block b in dorm.Blocks)
    {
        if (b.BlockName == blockName)
            return b;
    }
    return null;
}
public Dormitory GetDormitory(string dormName)
{
    foreach (Dormitory d in _dormitories)
    {
        if (d.Name == dormName)
            return d;
    }
    return null;
}

}

class Program
{
    static void Main()
    {
    }
} 