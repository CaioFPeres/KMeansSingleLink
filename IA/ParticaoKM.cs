using System;
using System.Collections.Generic;

public class ParticaoKM : Particao
{
  //variavel que salva a lista de cluster e pontos para o uso do kmedias
  private List<List<Ponto>> clusterList;

  //particao para kMedias
  public ParticaoKM(int numEle, int numClust, Ponto[] dataset)
  {
    //salva o numero de clusters
    numCluster = numClust;

    //variavel usada para randomizar os pontos para o começo do kmedias
    var rng = new Random();
    //variavel que salvo o numero de pontos
    int n = dataset.Length;
    //while que percorre todos os pontos
    while (n > 1) 
    {
      //variavel que determina um indice aleatorio para trocar a posição
      //do indice atual
      int z = rng.Next(n--);
      Ponto temp = dataset[n];
      dataset[n] = dataset[z];
      dataset[z] = temp;
    }
    
    
    //cria a lista de clusters e pontos
    this.clusterList = new List<List<Ponto>>();
    
    //for que separa os pontos em clusters
    for (int i = 0; i < numClust; i++)
    {
      
      //variaveis que determinam quais pontos vão para cluster atual
      int pontoAtual = (numEle/numClust) * i;
      int fimClus = (numEle/numClust) * (i + 1);
      
      //cria a lista de pontos do cluster atual
      List<Ponto> cluster = new List<Ponto>();
      
      //for que percorre os pontos que serão adicionados ao ponto atual
      for(int j = pontoAtual; (j < fimClus && j < numEle) || (i == numClust-1 && j < numEle); j++){
        //adiciona o ponto ao cluster atual
        cluster.Add(dataset[j]);
      }
      
      //adiciona o cluster a lista de clusters
      clusterList.Add(cluster);
    }
  }

  //função que move um ponto para outro cluster
  public void trocarPontoCluster(int idPonto, int idClusterIni, int idClusteNovo){
    //variavel que salva o ponto que sera transferido
    Ponto ponto = clusterList[idClusterIni][idPonto];
    //remove o ponto do cluster atual
    clusterList[idClusterIni].RemoveAt(idPonto);
    //insere o ponto no novo cluster
    clusterList[idClusteNovo].Add(ponto);
  }
    
  //função que retorna a lista de clusters
  public List<List<Ponto>> getClusterList(){
    return clusterList;
  }
}