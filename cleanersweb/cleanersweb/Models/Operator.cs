using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Operator
/// </summary>
public class Operator
{
    private string _fullName;
    private string _pin;
    private string _jobType;
    private string _shift;

    public string FullName
    {
        get => _fullName;
        set
        {
            _fullName = value;
        }
    }

    public string Pin
    {
        get => _pin;
        set
        {
            _pin = value;
        }
    }

    public string JobType
    {

        get => _jobType;
        set
        {
            _jobType = value;
        }
    }

    public string Shift
    {
        get => _shift;
        set
        {
            _shift = value;
        }
    }
}