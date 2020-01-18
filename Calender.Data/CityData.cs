using System;
using System.Collections.Generic;
using System.Text;

namespace Calender.Data
{
    public enum FieldType
    {
        None,
        Sunrise,
        Sunset,
        Moonrise,
        TamilYear,
        NorthYear,
        GujaratYear,
        Ayana,
        Ritu,
        VedicRitu,
        TamilMonth,
        SanskritMonth,
        Paksha,
        Tithi,
        Nakshatra,
        Yoga,
        Karana,
        SunRasi,
        MoonRasi,
        RahuKalam,
        YamaGandam,
        Gulikai,
        Festival,
        MaxFields
    };
    public enum TimeZoneValues
    {
        Hawaii,
        PST,
        MST,
        CST,
        EST,
        Trinidad,
        UK,
        India,
        Malaysia,
        WAU,
        SAU,
        NT,
        QSLND,
        NSW,
        ACT,
        Victoria,
        NZ,
        Fiji,
        Unknown,
        MaxTimeZoneValue
    };
    class StateOrCity
    {
        public String _Name;
    };

    class City : StateOrCity
    {
        public String _UrlToken;// Valid if its a city
        public TimeZoneValues _timeZone;
        public City(String Name, String UrlToken)
        {
            _Name = Name;
            _UrlToken = UrlToken;
            _timeZone = TimeZoneValues.Unknown;
        }
        public City(String Name, String UrlToken, TimeZoneValues timeZone)
        {
            _Name = Name;
            _timeZone = timeZone;
            _UrlToken = UrlToken;
        }
    };

    class State : StateOrCity
    {
        public City[] _cities; // Valid if its a state
        public TimeZoneValues _timeZone;
        public State(String Name, City[] cities, TimeZoneValues timeZone)
        {
            _Name = Name;
            _cities = cities;
            _timeZone = timeZone;
        }
    };

    class SubContinent
    {
        public String _Name;
        public StateOrCity[] _stateOrCityList;
        public TimeZoneValues _timeZone;
        public SubContinent(String name, StateOrCity[] stateOrCityList, TimeZoneValues timeZone)
        {
            _Name = name;
            _stateOrCityList = stateOrCityList;
            _timeZone = timeZone;
        }
    };


    public class LatLong
    {
        public String _UrlToken;
        public double _Latitude;
        public double _Longtitude;
        public LatLong(String urlToken, double latitude, double longtitude)
        {
            _UrlToken = urlToken;
            _Latitude = latitude;
            _Longtitude = longtitude;
        }
    }

    class CityData
    {
        public static SubContinent[] GetCityData()
        {
            City[] India = {
                new City("Hyderabad, AP", "Hyderabad-AP-India"),
            };
            SubContinent[] subContinents = {
                new SubContinent (  "India", India , TimeZoneValues.India),
            };

            return subContinents;
        }
    }
}