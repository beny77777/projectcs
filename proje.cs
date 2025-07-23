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
    public enum EquipmentStatus { Healthy, Damaged, InRepair, Broken, Repairing }


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
     public Dormitory GetDormitory(string dormName)
    {
        return Dormitories.Find(d => d.Name.Equals(dormName, StringComparison.OrdinalIgnoreCase));
    }

    public Block GetBlock(string dormName, string blockName)
    {
        var dorm = GetDormitory(dormName);
        if (dorm == null) return null;
        return dorm.Blocks.Find(b => b.BlockName.Equals(blockName, StringComparison.OrdinalIgnoreCase));
    }

    public bool RemoveBlock(string dormName, string blockName)
    {
        var dorm = GetDormitory(dormName);
        if (dorm == null) return false;
        var block = dorm.Blocks.Find(b => b.BlockName.Equals(blockName, StringComparison.OrdinalIgnoreCase));
        if (block == null) return false;
        dorm.Blocks.Remove(block);
        return true;
    }

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

   

    public bool AddBlockToDormitory(string dormName, Block block)
    {
        Dormitory dorm = GetDormitory(dormName);
        if (dorm == null) return false;

        block.ParentDormitory = dorm;
        dorm.Blocks.Add(block);
        return true;
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
        equipment.AssignedRoom = null; 
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
    //  متدهای مدیریت بلوک
    static void AddBlock(DormitorySystem system)
    {
        Console.WriteLine("----- Add New Block -----");

        Console.Write("Enter Dormitory Name: ");
        string dormName = Console.ReadLine();
        var dorm = system.GetDormitory(dormName);
        if (dorm == null)
        {
            Console.WriteLine("Dormitory not found.");
            return;
        }

        Console.Write("Enter Block Name: ");
        string blockName = Console.ReadLine();

        Console.Write("Number of Floors: ");
        if (!int.TryParse(Console.ReadLine(), out int floors) || floors <= 0)
        {
            Console.WriteLine("Invalid number of floors.");
            return;
        }

        Console.Write("Number of Rooms: ");
        if (!int.TryParse(Console.ReadLine(), out int roomsCount) || roomsCount <= 0)
        {
            Console.WriteLine("Invalid number of rooms.");
            return;
        }

        Block newBlock = new Block
        {
            BlockName = blockName,
            FloorCount = floors,
            RoomCount = roomsCount,
            ParentDormitory = dorm
        };

        // اضافه کردن اتاق‌ها به بلوک
        for (int i = 1; i <= roomsCount; i++)
        {
            Room room = new Room
            {
                RoomNumber = $"{blockName}-{i}",
                Floor = (i - 1) % floors + 1,
                Capacity = 6,
                ParentBlock = newBlock
            };
            newBlock.Rooms.Add(room);
        }

        dorm.Blocks.Add(newBlock);
        Console.WriteLine($"Block {blockName} added successfully to dormitory {dormName}.");
    }

    static void DeleteBlock(DormitorySystem system)
    {
        Console.WriteLine("----- Delete Block -----");

        Console.Write("Enter Dormitory Name: ");
        string dormName = Console.ReadLine();

        Console.Write("Enter Block Name: ");
        string blockName = Console.ReadLine();

        bool result = system.RemoveBlock(dormName, blockName);
        if (result)
            Console.WriteLine("Block deleted successfully.");
        else
            Console.WriteLine("Block or dormitory not found.");
    }

    static void ShowBlocks(DormitorySystem system)
    {
        Console.WriteLine("----- Show Blocks -----");

        Console.Write("Enter Dormitory Name: ");
        string dormName = Console.ReadLine();

        var dorm = system.GetDormitory(dormName);
        if (dorm == null)
        {
            Console.WriteLine("Dormitory not found.");
            return;
        }

        if (dorm.Blocks.Count == 0)
        {
            Console.WriteLine("No blocks in this dormitory.");
            return;
        }

        foreach (var block in dorm.Blocks)
        {
            Console.WriteLine($"Block: {block.BlockName}, Floors: {block.FloorCount}, Rooms: {block.RoomCount}");
        }
    }

    static void EditBlock(DormitorySystem system)
    {
        Console.WriteLine("----- Edit Block -----");

        Console.Write("Enter Dormitory Name: ");
        string dormName = Console.ReadLine();
        var dorm = system.GetDormitory(dormName);
        if (dorm == null)
        {
            Console.WriteLine("Dormitory not found.");
            return;
        }

        Console.Write("Enter Block Name: ");
        string blockName = Console.ReadLine();
        var block = system.GetBlock(dormName, blockName);
        if (block == null)
        {
            Console.WriteLine("Block not found.");
            return;
        }

        Console.Write($"New Block Name (leave empty to keep '{block.BlockName}'): ");
        string newName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newName))
            block.BlockName = newName;

        Console.Write($"New Floor Count (leave empty to keep {block.FloorCount}): ");
        string newFloorsStr = Console.ReadLine();
        if (int.TryParse(newFloorsStr, out int newFloors) && newFloors > 0)
            block.FloorCount = newFloors;

        Console.Write($"New Room Count (leave empty to keep {block.RoomCount}): ");
        string newRoomsStr = Console.ReadLine();
        if (int.TryParse(newRoomsStr, out int newRooms) && newRooms > 0)
            block.RoomCount = newRooms;

        Console.WriteLine("Block updated.");
    }

    // تابع های مدییریت اشخاص

    static void AddStudent(DormitorySystem system)
    {
        Console.WriteLine("----- Add Student -----");

        Console.Write("Full Name: ");
        string fullName = Console.ReadLine();

        Console.Write("National ID: ");
        string id = Console.ReadLine();

        Console.Write("Student ID: ");
        string studentId = Console.ReadLine();

        Student student = new Student(fullName, id, studentId);

        system.AddStudent(student);
        Console.WriteLine("Student added successfully.");
    }

    static void ShowStudents(DormitorySystem system)
    {
        Console.WriteLine("----- Students List -----");

        if (system.Students.Count == 0)
        {
            Console.WriteLine("No students registered.");
            return;
        }

        foreach (var student in system.Students)
        {
            Console.WriteLine($"Name: {student.FullName}, National ID: {student.Id}, Student ID: {student.StudentId}");
        }
    }

    static void RemoveStudent(DormitorySystem system)
    {
        Console.WriteLine("----- Remove Student -----");

        Console.Write("Enter National ID of student to remove: ");
        string id = Console.ReadLine();

        Student toRemove = null;
        foreach (var student in system.Students)
        {
            if (student.Id == id)
            {
                toRemove = student;
                break;
            }
        }

        if (toRemove != null)
        {
            system.RemoveStudent(toRemove);
            Console.WriteLine("Student removed.");
        }
        else
        {
            Console.WriteLine("Student with this National ID not found.");
        }
    }

    static void AddDormitoryManager(DormitorySystem system)
    {
        Console.WriteLine("----- Add Dormitory Manager -----");

        Console.Write("Full Name: ");
        string fullName = Console.ReadLine();

        Console.Write("National ID: ");
        string id = Console.ReadLine();

        DormitoryManager manager = new DormitoryManager
        {
            FullName = fullName,
            Id = id,
            Position = "Dormitory Manager"
        };

        Console.Write("Assigned Dormitory Name: ");
        string dormName = Console.ReadLine();

        var dorm = system.GetDormitory(dormName);
        if (dorm == null)
        {
            Console.WriteLine("Dormitory not found.");
            return;
        }

        manager.AssignedDormitory = dorm;
        system.DormitoryManagers.Add(manager);

        dorm.Manager = manager;

        Console.WriteLine("Dormitory manager added.");
    }

    static void ShowDormitoryManagers(DormitorySystem system)
    {
        Console.WriteLine("----- Dormitory Managers List -----");
        if (system.DormitoryManagers.Count == 0)
        {
            Console.WriteLine("No dormitory managers registered.");
            return;
        }

        foreach (var m in system.DormitoryManagers)
        {
            Console.WriteLine($"Name: {m.FullName}, National ID: {m.Id}, Dormitory: {m.AssignedDormitory?.Name}");
        }
    }

    static void AddBlockManager(DormitorySystem system)
    {
        Console.WriteLine("----- Add Block Manager -----");

        Console.Write("Full Name: ");
        string fullName = Console.ReadLine();

        Console.Write("National ID: ");
        string id = Console.ReadLine();

        BlockManager manager = new BlockManager
        {
            FullName = fullName,
            Id = id,
            Position = "Block Manager"
        };

        Console.Write("Assigned Block Name: ");
        string blockName = Console.ReadLine();

        Block assignedBlock = null;
        foreach (var dorm in system.Dormitories)
        {
            assignedBlock = dorm.Blocks.Find(b => b.BlockName == blockName);
            if (assignedBlock != null) break;
        }

        if (assignedBlock == null)
        {
            Console.WriteLine("Block not found.");
            return;
        }

        manager.AssignedBlock = assignedBlock;
        system.BlockManagers.Add(manager);

        assignedBlock.Manager = manager;

        Console.WriteLine("Block manager added.");
    }

    static void ShowBlockManagers(DormitorySystem system)
    {
        Console.WriteLine("----- Block Managers List -----");
        if (system.BlockManagers.Count == 0)
        {
            Console.WriteLine("No block managers registered.");
            return;
        }

        foreach (var m in system.BlockManagers)
        {
            Console.WriteLine($"Name: {m.FullName}, National ID: {m.Id}, Block: {m.AssignedBlock?.BlockName}");
        }
    }

    //  تابع های مدیریت تجهیزات 

    static void AddEquipment(DormitorySystem system)
    {
        Console.WriteLine("----- Add New Equipment -----");

        Console.Write("Equipment Name: ");
        string name = Console.ReadLine();

        Console.WriteLine("Select Equipment Type:");
        Console.WriteLine("1. Bed");
        Console.WriteLine("2. Closet");
        Console.WriteLine("3. Table");
        Console.WriteLine("4. Chair");
        Console.WriteLine("5. Refrigerator");
        Console.Write("Choice: ");
        string typeInput = Console.ReadLine();

        Equipment.EquipmentType type;
        switch (typeInput)
        {
            case "1": type = Equipment.EquipmentType.Bed; break;
            case "2": type = Equipment.EquipmentType.Closet; break;
            case "3": type = Equipment.EquipmentType.Table; break;
            case "4": type = Equipment.EquipmentType.Chair; break;
            case "5": type = Equipment.EquipmentType.Refrigerator; break;
            default:
                Console.WriteLine("Invalid equipment type.");
                return;
        }

        Console.Write("Part Number: ");
        string partNumber = Console.ReadLine();

        Console.Write("Asset Number: ");
        string assetNumber = Console.ReadLine();

        Equipment equipment = new Equipment
        {
            Name = name,
            Type = type,
            PartNumber = partNumber,
            AssetNumber = assetNumber,
            Status = Equipment.EquipmentStatus.Healthy
        };

        system.Equipments.Add(equipment);
        Console.WriteLine("Equipment added successfully.");
    }

    static void ShowEquipments(DormitorySystem system)
    {
        Console.WriteLine("----- Equipment List -----");

        if (system.Equipments.Count == 0)
        {
            Console.WriteLine("No equipment registered.");
            return;
        }

        foreach (var eq in system.Equipments)
        {
            Console.WriteLine($"Name: {eq.Name}, Type: {eq.Type}, Part Number: {eq.PartNumber}, Asset Number: {eq.AssetNumber}, Status: {eq.Status}");
        }
    }

    static void RemoveEquipment(DormitorySystem system)
    {
        Console.WriteLine("----- Remove Equipment -----");

        Console.Write("Enter Asset Number of equipment to remove: ");
        string assetNumber = Console.ReadLine();

        Equipment toRemove = null;
        foreach (var eq in system.Equipments)
        {
            if (eq.AssetNumber == assetNumber)
            {
                toRemove = eq;
                break;
            }
        }

        if (toRemove != null)
        {
            system.Equipments.Remove(toRemove);
            Console.WriteLine("Equipment removed.");
        }
        else
        {
            Console.WriteLine("Equipment with this Asset Number not found.");
        }
    }

    // گزارش ها

    static void ReportCapacity(DormitorySystem system)
    {
        Console.WriteLine("----- Dormitory Capacity Report -----");
        foreach (var dorm in system.Dormitories)
        {
            int totalCapacity = 0;
            int currentOccupancy = 0;
            foreach (var block in dorm.Blocks)
            {
                foreach (var room in block.Rooms)
                {
                    totalCapacity += room.Capacity;
                    currentOccupancy += room.Residents.Count;
                }
            }
            Console.WriteLine($"Dormitory: {dorm.Name}, Total Capacity: {totalCapacity}, Occupied: {currentOccupancy}");
        }
    }

    static void ReportEquipmentStatus(DormitorySystem system)
    {
        Console.WriteLine("----- Equipment Status Report -----");

        int healthyCount = 0, brokenCount = 0, repairingCount = 0;
        foreach (var eq in system.Equipments)
        {
            switch (eq.Status)
            {
                case Equipment.EquipmentStatus.Healthy: healthyCount++; break;
                case Equipment.EquipmentStatus.Broken: brokenCount++; break;
                case Equipment.EquipmentStatus.Repairing: repairingCount++; break;
            }
        }

        Console.WriteLine($"Healthy: {healthyCount}, Broken: {brokenCount}, Repairing: {repairingCount}");
    }

    static void ReportStudentsInDormitory(DormitorySystem system)
    {
        Console.WriteLine("----- Students in Dormitory Report -----");

        Console.Write("Enter Dormitory Name: ");
        string dormName = Console.ReadLine();

        var dorm = system.GetDormitory(dormName);
        if (dorm == null)
        {
            Console.WriteLine("Dormitory not found.");
            return;
        }

        Console.WriteLine($"Students in dormitory {dorm.Name}:");

        foreach (var block in dorm.Blocks)
        {
            foreach (var room in block.Rooms)
            {
                foreach (var student in room.Residents)
                {
                    Console.WriteLine($"{student.FullName} - Room: {room.RoomNumber}");
                }
            }
        }
    }

    //منوی اصلی

    static void Main(string[] args)
    {
        DormitorySystem system = new DormitorySystem();

        while (true)
        {
            Console.WriteLine("----- Dormitory Management System -----");
            Console.WriteLine("1. Block Management");
            Console.WriteLine("2. Person Management");
            Console.WriteLine("3. Equipment Management");
            Console.WriteLine("4. Reports");
            Console.WriteLine("5. Exit");
            Console.Write("Select an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("----- Block Management -----");
                    Console.WriteLine("1. Add Block");
                    Console.WriteLine("2. Delete Block");
                    Console.WriteLine("3. Show Blocks");
                    Console.WriteLine("4. Edit Block");
                    Console.WriteLine("5. Back");
                    Console.Write("Select an option: ");
                    string blockChoice = Console.ReadLine();
                    switch (blockChoice)
                    {
                        case "1": AddBlock(system); break;
                        case "2": DeleteBlock(system); break;
                        case "3": ShowBlocks(system); break;
                        case "4": EditBlock(system); break;
                        case "5": break;
                        default: Console.WriteLine("Invalid choice"); break;
                    }
                    break;

                case "2":
                    Console.WriteLine("----- Person Management -----");
                    Console.WriteLine("1. Add Student");
                    Console.WriteLine("2. Remove Student");
                    Console.WriteLine("3. Show Students");
                    Console.WriteLine("4. Add Dormitory Manager");
                    Console.WriteLine("5. Show Dormitory Managers");
                    Console.WriteLine("6. Add Block Manager");
                    Console.WriteLine("7. Show Block Managers");
                    Console.WriteLine("8. Back");
                    Console.Write("Select an option: ");
                    string personChoice = Console.ReadLine();
                    switch (personChoice)
                    {
                        case "1": AddStudent(system); break;
                        case "2": RemoveStudent(system); break;
                        case "3": ShowStudents(system); break;
                        case "4": AddDormitoryManager(system); break;
                        case "5": ShowDormitoryManagers(system); break;
                        case "6": AddBlockManager(system); break;
                        case "7": ShowBlockManagers(system); break;
                        case "8": break;
                        default: Console.WriteLine("Invalid choice"); break;
                    }
                    break;

                case "3":
                    Console.WriteLine("----- Equipment Management -----");
                    Console.WriteLine("1. Add Equipment");
                    Console.WriteLine("2. Remove Equipment");
                    Console.WriteLine("3. Show Equipments");
                    Console.WriteLine("4. Back");
                    Console.Write("Select an option: ");
                    string equipmentChoice = Console.ReadLine();
                    switch (equipmentChoice)
                    {
                        case "1": AddEquipment(system); break;
                        case "2": RemoveEquipment(system); break;
                        case "3": ShowEquipments(system); break;
                        case "4": break;
                        default: Console.WriteLine("Invalid choice"); break;
                    }
                    break;

                case "4":
                    Console.WriteLine("----- Reports -----");
                    Console.WriteLine("1. Dormitory Capacity Report");
                    Console.WriteLine("2. Equipment Status Report");
                    Console.WriteLine("3. Students in Dormitory Report");
                    Console.WriteLine("4. Back");
                    Console.Write("Select an option: ");
                    string reportChoice = Console.ReadLine();
                    switch (reportChoice)
                    {
                        case "1": ReportCapacity(system); break;
                        case "2": ReportEquipmentStatus(system); break;
                        case "3": ReportStudentsInDormitory(system); break;
                        case "4": break;
                        default: Console.WriteLine("Invalid choice"); break;
                    }
                    break;

                case "5":
                    Console.WriteLine("Exiting...");
                    return;

                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }
    }
}

