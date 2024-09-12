using System;

public class SatelliteHideFilter
{
    private int? _minYearFilter;
    private String _nameFilter;

    public SatelliteHideFilter(int? minYear = null, String name = null) {
        _minYearFilter = minYear;
        _nameFilter = name;
    }

    public bool ShouldShowSatellite(Satellite satellite) {
        if (_minYearFilter.HasValue && satellite.TLE.getStartYear() < _minYearFilter.Value) return false;
        if (_nameFilter!= null && !satellite.TLE.getName().Contains(_nameFilter)) return false;
        return true;
    }
}
