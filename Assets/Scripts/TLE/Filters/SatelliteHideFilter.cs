using System.Collections.Generic;
using TLE.Filters;

public class SatelliteHideFilter : Filter {
    private readonly int? _minYearFilter;
    private readonly string _nameFilter;
    private readonly SatelliteType _satelliteType;

    public SatelliteHideFilter(int? minYearFilter = null, string nameFilter = null, SatelliteType satelliteType = SatelliteType.NULL) {
        _minYearFilter = minYearFilter;
        _nameFilter = nameFilter;
        _satelliteType = satelliteType;
    }

    public bool ShouldShowSatellite(Satellite satellite) {
        if (_minYearFilter.HasValue && satellite.TLE.getStartYear() < _minYearFilter.Value) return false;
        if (_nameFilter != null && !satellite.TLE.getName().Contains(_nameFilter)) return false;
        if (_satelliteType != SatelliteType.NULL && satellite.SatelliteType != _satelliteType) return false;

        return true;
    }

    public static SatelliteCompositeHideFilter operator &(SatelliteHideFilter a, SatelliteHideFilter b) {
        return new SatelliteCompositeHideFilter(new List<SatelliteHideFilter> { a, b });
    }

    public override string ToString() {
        return $"Filter({_nameFilter}, {_satelliteType})";
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;
        
        return ToString() == obj.ToString();
    }
    
    public override int GetHashCode()
    {
        return _nameFilter.GetHashCode() + _satelliteType.GetHashCode(); 
    }
}