public interface Filter {
    public bool ShouldShowSatellite(Satellite satellite);
    public static Filter operator&(Filter a, Filter b) {
        return new SatelliteHideFilter();
    }
}