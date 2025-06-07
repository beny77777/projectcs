using System;
using System.Collections.Generic;
class Person
{
    public string FullName { get; set; } 
    public string Id { get; set; } 
    public string PhoneNumber { get; set; } 
    public string Address { get; set; } /
}

public class Student : Person
{
    
    private string _studentId;
    private Room _Room;
    private Block _residenceBlock;
    private Dormitory _residenceDormitory;
    private List<Equipment> _personalItems = new();

   
    public string StudentId
    {
        get { return _studentId; }
        set { _studentId = value; }
    }

   
    public Room Room
    {
        get { return _Room; }
        set { _Room = value; }
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


class Program
{
    static void Main()
    {
    }
} 

    
