using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SatelliteCompositeHideFilter : Filter
{

    private List<SatelliteHideFilter> _andFilters = new List<SatelliteHideFilter>();

    public SatelliteCompositeHideFilter(List<SatelliteHideFilter> andFilters = null){
        andFilters?.ForEach(x => _andFilters.Add(x));
    }

    public bool ShouldShowSatellite(Satellite satellite) {
        foreach (var filter in _andFilters) {
            if (!filter.ShouldShowSatellite(satellite)) return false;
        }
        
        return true;
    }

    public static SatelliteCompositeHideFilter operator& (SatelliteCompositeHideFilter a, SatelliteHideFilter b) {
        SatelliteCompositeHideFilter filter = new SatelliteCompositeHideFilter(a._andFilters);
        filter._andFilters.Add(b);    

        return filter;
    }

    public static SatelliteCompositeHideFilter operator& (SatelliteCompositeHideFilter a, SatelliteCompositeHideFilter b) {
        SatelliteCompositeHideFilter filter = new SatelliteCompositeHideFilter(a._andFilters);
        b._andFilters.ForEach(x => filter._andFilters.Add(x));    

        return filter;
    }

    public static SatelliteCompositeHideFilter operator- (SatelliteCompositeHideFilter a, SatelliteHideFilter b) {
        SatelliteCompositeHideFilter filter = new SatelliteCompositeHideFilter(a._andFilters);
        filter._andFilters.RemoveAll(x => x.Equals(b));
        
        return filter;
    }

    public override string ToString() {
        return $"SatelliteCompositeHideFilter({_andFilters.Count})";
    }

    public List<SatelliteHideFilter> GetFilters() {
        return _andFilters;
    }
}
