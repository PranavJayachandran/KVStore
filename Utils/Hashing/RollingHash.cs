namespace Utils;

public class RollingHash : IHash {
  int maxVal = 0;
  int p;
  public RollingHash(int maxValue, int p = 31){
    this.maxVal = maxValue;
    this.p = p;
  }
  public int GetHash (string val){
    int hash = 0;
    int rollingP = 1;
    for(int i=0;i<val.Length;i++){
      hash = (val[i] * rollingP) % maxVal;
      rollingP = (rollingP * p) % maxVal;
    }
    return hash;
  }
}
