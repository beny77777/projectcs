using System;
using System.Collections.Generic;

public class Person
{
    public string FullName { get; set; }
    public string Id { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}

public class Student : Person
{
    private string studentId;
    private Room assignedRoom;
    private Block residenceBlock;
    private Dormitory residenceDormitory;
    private List<Equipment> personalItems = new List<Equipment>();

    public Student() { }

    public Student(string fullName, string id, string studentId)
    {
        FullName = fullName;
        Id = id;
        this.studentId = studentId;
    }

    public string StudentId
    {
        get { return studentId; }
        set { studentId = value; }
    }

    public Room AssignedRoom
    {
        get { return assignedRoom; }
        set { assignedRoom = value; }
    }

    public Block ResidenceBlock
    {
        get { return residenceBlock; }
        set { residenceBlock = value; }
    }

    public Dormitory ResidenceDormitory
    {
        get { return residenceDormitory; }
        set { residenceDormitory = value; }
    }

    public List<Equipment> PersonalItems
    {
        get { return personalItems; }
        set { personalItems = value ?? new List<Equipment>(); }
    }
}

public class Block
{
    private string blockName;
    private int floorCount;
    private int roomCount;
    private BlockManager manager;
    private List<Room> rooms = new List<Room>();
    private Dormitory parentDormitory;

    public Block() { }

    public string BlockName
    {
        get { return blockName; }
        set { blockName = value; }
    }

    public int FloorCount
    {
        get { return floorCount; }
        set { floorCount = value; }
    }

    public int RoomCount
    {
        get { return roomCount; }
        set { roomCount = value; }
    }

    public BlockManager Manager
    {
        get { return manager; }
        set { manager = value; }
    }

    public List<Room> Rooms
    {
        get { return rooms; }
        set { rooms = value ?? new List<Room>(); }
    }

    public Dormitory ParentDormitory
    {
        get { return parentDormitory; }
        set { parentDormitory = value; }
    }
}

public class Equipment
{
    public enum EquipmentType { Bed, Closet, Table, Chair, Refrigerator }
    public enum EquipmentStatus { Healthy, Damaged, InRepair }

    public string Name { get; set; }
    public EquipmentType Type { get; set; }
    public string PartNumber { get; set; }
    public string AssetNumber { get; set; }
    public EquipmentStatus Status { get; set; }

    public Room AssignedRoom { get; set; }
    public Student OwnerStudent { get; set; }
}

public class Room
{
    public string RoomNumber { get; set; }
    public int Floor { get; set; }
    public int Capacity { get; set; } = 6; // max 6 residents

    private List<Student> residents = new List<Student>();
    private List<Equipment> equipments = new List<Equipment>();

    public Room() { }

    public List<Equipment> Equipments
    {
        get { return equipments; }
        set { equipments = value ?? new List<Equipment>(); }
    }

    public List<Student> Residents
    {
        get { return residents; }
        set { residents = value ?? new List<Student>(); }
    }

    public Block ParentBlock { get; set; }

    public int CurrentCapacity()
    {
        return Residents.Count;
    }

    public bool IsFull
    {
        get
        {
            return Residents.Count >= Capacity;
        }
    }
}

public class Dormitory
{
    public string Name { get; set; }
    public string Address { get; set; }
    public int TotalCapacity { get; set; }

    private DormitoryManager manager;
    private List<Block> blocks = new List<Block>();

    public Dormitory() { }

    public DormitoryManager Manager
    {
        get { return manager; }
        set { manager = value; }
    }

    public List<Block> Blocks
    {
        get { return blocks; }
        set { blocks = value ?? new List<Block>(); }
    }

    public int CurrentCapacity()
    {
        int count = 0;
        foreach (var block in Blocks)
        {
            foreach (var room in block.Rooms)
            {
                count += room.CurrentCapacity();
            }
        }
        return count;
    }

    public int RemainingCapacity()
    {
        return TotalCapacity - CurrentCapacity();
    }
}

public class DormitoryManager : Person
{
    public string Position { get; set; }
    public Dormitory AssignedDormitory { get; set; }
}

public class BlockManager : Person
{
    public string Position { get; set; }
    public Block AssignedBlock { get; set; }
}

public class DormitorySystem
{
    private List<Dormitory> dormitories = new List<Dormitory>();
    private List<Student> students = new List<Student>();
    private List<DormitoryManager> dormManagers = new List<DormitoryManager>();
    private List<BlockManager> blockManagers = new List<BlockManager>();
    private List<Equipment> allEquipments = new List<Equipment>();

    public List<Dormitory> Dormitories => dormitories;
    public List<Student> Students => students;
    public List<DormitoryManager> DormitoryManagers => dormManagers;
    public List<BlockManager> BlockManagers => blockManagers;
    public List<Equipment> Equipments => allEquipments;

    public Equipment GetEquipmentByName(string name)
    {
        foreach (var eq in allEquipments)
        {
            if (eq.Name == name)
                return eq;
        }
        return null;
    }

    public void AddEquipment(Equipment equipment)
    {
        if (equipment != null && !allEquipments.Contains(equipment))
            allEquipments.Add(equipment);
    }

    public bool RemoveEquipment(Equipment equipment)
    {
        if (equipment != null && allEquipments.Contains(equipment))
        {
            allEquipments.Remove(equipment);
            return true;
        }
        return false;
    }

    public DormitorySystem() { }

    public void AddStudent(Student student)
    {
        if (student != null && !students.Contains(student))
            students.Add(student);
    }

    public void AddDormitory(Dormitory dormitory)
    {
        if (dormitory != null && !dormitories.Contains(dormitory))
            dormitories.Add(dormitory);
    }

    public void AddBlockToDormitory(Block block, Dormitory dormitory)
    {
        if (block != null && dormitory != null && dormitories.Contains(dormitory))
        {
            dormitory.Blocks.Add(block);
            block.ParentDormitory = dormitory;
        }
    }

    public void RemoveStudent(Student student)
    {
        if (student != null && students.Contains(student))
        {
            students.Remove(student);
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
        foreach (Dormitory d in dormitories)
        {
            if (d.Name == dormName)
                return d;
        }
        return null;
    }

    public bool TransferEquipment(Equipment equipment, Room newRoom)
    {
        if (equipment == null || newRoom == null)
            return false;

        Room oldRoom = equipment.AssignedRoom;

        if (oldRoom != null)
            oldRoom.Equipments.Remove(equipment);

        newRoom.Equipments.Add(equipment);
        equipment.AssignedRoom = newRoom;
        equipment.OwnerStudent = null; // because it moved to room

        return true;
    }

    public bool TransferStudent(Student student, Room newRoom)
    {
        if (student == null || newRoom == null)
            return false;

        if (newRoom.IsFull)
        {
            Console.WriteLine("Room is full. Transfer not possible.");
            return false;
        }

        Room oldRoom = student.AssignedRoom;
        if (oldRoom != null)
            oldRoom.Residents.Remove(student);

        newRoom.Residents.Add(student);
        student.AssignedRoom = newRoom;

        return true;
    }

    public int GetRemainingCapacity(string dormName)
    {
        Dormitory dorm = GetDormitory(dormName);
        if (dorm == null)
            return -1;

        int total = 0;
        foreach (Block block in dorm.Blocks)
        {
            foreach (Room room in block.Rooms)
            {
                total += room.Capacity - room.Residents.Count;
            }
        }
        return total;
    }

    public bool AssignEquipmentToRoom(string equipmentName, Room room)
    {
        var equipment = GetEquipmentByName(equipmentName);
        if (equipment == null || room == null)
            return false;

        if (equipment.AssignedRoom != null)
            equipment.AssignedRoom.Equipments.Remove(equipment);

        if (equipment.OwnerStudent != null)
            equipment.OwnerStudent.PersonalItems.Remove(equipment);

        equipment.AssignedRoom = room;
        equipment.OwnerStudent = null; // assigned only to room
        room.Equipments.Add(equipment);
        return true;
    }

    public bool AssignEquipmentToStudent(string equipmentName, Student student)
    {
        var equipment = GetEquipmentByName(equipmentName);
        if (equipment == null || student == null)
            return false;

        if (equipment.AssignedRoom != null)
            equipment.AssignedRoom.Equipments.Remove(equipment);

        if (equipment.OwnerStudent != null)
            equipment.OwnerStudent.PersonalItems.Remove(equipment);

        equipment.OwnerStudent = student;
        equipment.AssignedRoom = null; // assigned only to student
        student.PersonalItems.Add(equipment);
        return true;
    }

    public bool UnassignEquipment(string equipmentName)
    {
        var equipment = GetEquipmentByName(equipmentName);
        if (equipment == null)
            return false;

        equipment.AssignedRoom?.Equipments.Remove(equipment);
        equipment.OwnerStudent?.PersonalItems.Remove(equipment);
        equipment.AssignedRoom = null;
        equipment.OwnerStudent = null;
        return true;
    }
}

class Program
{
    static void Main()
    {
        // ساخت سیستم خوابگاه
        DormitorySystem system = new DormitorySystem();

        // ایجاد خوابگاه
        Dormitory dorm = new Dormitory
        {
            Name = "Central Dorm",
            Address = "123 University Ave",
            TotalCapacity = 100
        };

        // ایجاد مدیر خوابگاه و انتساب به خوابگاه
        DormitoryManager manager = new DormitoryManager
        {
            FullName = "Ali Rezaei",
            Id = "DM001",
            PhoneNumber = "09121234567",
            Address = "Tehran",
            Position = "Dormitory Manager",
            AssignedDormitory = dorm
        };
        dorm.Manager = manager;

        // ایجاد بلوک و مدیر بلوک و افزودن آن به خوابگاه
        Block block = new Block
        {
            BlockName = "A",
            FloorCount = 3,
            RoomCount = 2,
            Manager = new BlockManager
            {
                FullName = "Sara Ahmadi",
                Id = "BM001",
                PhoneNumber = "09351234567",
                Address = "Tehran",
                Position = "Block Manager"
            }
        };
        system.AddDormitory(dorm);
        system.AddBlockToDormitory(block, dorm);

        // ایجاد اتاق و افزودن به بلوک 
        Room room = new Room
        {
            RoomNumber = "101",
            Floor = 1,
            Capacity = 6
        };
        block.Rooms.Add(room);
        room.ParentBlock = block;

        // ایجاد دانشجو و انتساب به خوابگاه، بلوک و اتاق
        Student student = new Student("Mehdi Mohammadi", "ST001", "99123456")
        {
            PhoneNumber = "09123456789",
            Address = "Kerman",
            ResidenceDormitory = dorm,
            ResidenceBlock = block,
            AssignedRoom = room
        };
        room.Residents.Add(student);
        system.AddStudent(student);

        // ایجاد تجهیز متعلق به اتاق و افزودن به لیست تجهیزات اتاق و سیستم
        Equipment roomEquipment = new Equipment
        {
            Name = "Table1",
            Type = Equipment.EquipmentType.Table,
            PartNumber = "PT123",
            AssetNumber = "AST987",
            Status = Equipment.EquipmentStatus.Healthy,
            AssignedRoom = room
        };
        room.Equipments.Add(roomEquipment);
        system.AddEquipment(roomEquipment);

        // ایجاد تجهیز شخصی متعلق به دانشجو و افزودن به لیست تجهیزات شخصی دانشجو و سیستم
        Equipment personalItem = new Equipment
        {
            Name = "Laptop",
            Type = Equipment.EquipmentType.Table,
            PartNumber = "LT001",
            AssetNumber = "AST123",
            Status = Equipment.EquipmentStatus.Healthy,
            OwnerStudent = student
        };
        student.PersonalItems.Add(personalItem);
        system.AddEquipment(personalItem);

        // نمایش اطلاعات کلی سیستم
        Console.WriteLine("System initialized successfully.");
        Console.WriteLine("Dormitory name: " + dorm.Name);
        Console.WriteLine("Total capacity: " + dorm.TotalCapacity);
        Console.WriteLine("Used capacity: " + dorm.CurrentCapacity());
        Console.WriteLine("Remaining capacity: " + dorm.RemainingCapacity());
        Console.WriteLine("Student: " + student.FullName + " in Room " + room.RoomNumber);
        Console.WriteLine("Room Equipments: " + room.Equipments.Count);
        Console.WriteLine("Student Personal Equipments: " + student.PersonalItems.Count);
    }
}
