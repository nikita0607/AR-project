using System.Collections.Generic;
using TLE.Filters;

public class SatelliteHideFilter : Filter {
    private int? _minYearFilter;
    private string _nameFilter;
    private SatelliteType _satelliteType;

    public SatelliteHideFilter(int? minYearFilter = null, string nameFilter = null, SatelliteType satelliteType = SatelliteType.NULL) {
        _minYearFilter = minYearFilter;
        _nameFilter = nameFilter;
        _satelliteType = satelliteType;
    }

    public bool ShouldShowSatellite(Satellite satellite) {
        if (_minYearFilter.HasValue && satellite.TLE.getStartYear() < _minYearFilter.Value) return false;
        if (_nameFilter!= null && !satellite.TLE.getName().Contains(_nameFilter)) return false;
        if (_satelliteType != SatelliteType.NULL && satellite.SatelliteType != _satelliteType) return false;

        return true;
    }

    public static SatelliteCompositeHideFilter operator &(SatelliteHideFilter a, SatelliteHideFilter b) {
        return new SatelliteCompositeHideFilter(new List<SatelliteHideFilter> { a, b });
    }

    public override string ToString() {
        return $"Filter({_nameFilter})";
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;
        
        return GetHashCode() == obj.GetHashCode();
    }
    
    public override int GetHashCode()
    {
        return _minYearFilter.GetHashCode()*17 + _nameFilter.GetHashCode() + _satelliteType.GetHashCode(); 
    }
}