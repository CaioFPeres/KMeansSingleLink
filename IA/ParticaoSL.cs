using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ParticaoSL : Particao
{
  //variavel que guarda a lista de cluster usando a classe ClusterSL
  private List<ClusterSL> clusters;  

  //construtor da particao para single link
  public ParticaoSL(int numEle, Ponto[] dataset){
    //cria lista de clusters
    this.clusters = new List<ClusterSL>();
    //salva o numero incial de clusters
    numCluster = numEle;

    //for que salva todos os pontos em clusters diferentes
    for (int i = 0; i < numEle; i++)
    {
      //cira a lista de pontos para cada custer
      List<Ponto> ponto = new List<Ponto>();
      //adiciona um ponto a essa lista
      ponto.Add(dataset[i]);
      //cria um cluster para esse ponto
      ClusterSL cluster = new ClusterSL(ponto);
      //adiciona a lista de pontos ao cluster
      clusters.Add(cluster);
    }
    //for que calcula a distancia entre todos os clusters
    for(int i = 0; i < numCluster; i++){
      //lista que salva a ditancia do cluster atual a todos os outros clusters
      List<double> dist = new List<double>();
      //pega o ponto do cluster atual, 0 pois cada cluster tem apenas 1 ponto
      Ponto ponto1 = clusters.ElementAt(i).getPonto(0);
      //for que percorre todos os outros pontos
      for(int j = 0; j < numCluster; j++){
        //pega o ponto dos outros clusters
        Ponto ponto2 = clusters.ElementAt(j).getPonto(0);
        //calcula a distancia e salva na lista de distancias do cluster atual
        dist.Add(DistanciaEuclidiana(ponto1,ponto2));
      }
      //adiciona o cluster atual a lista de clusters da classe
      clusters.ElementAt(i).setDist(dist);
    }
  }

  //função que junta o cluster de id clusterId2 ao de id clusteId1
  public void juntarCluster(int clusteId1, int clusterId2){  
    //variaveis que guardam os dois clusters
    ClusterSL cluster1 = clusters.ElementAt(clusteId1);
    ClusterSL cluster2 = clusters.ElementAt(clusterId2);
    //for que percorre todos os ids dos clusters para decidir qual distancia será mantida após a junção dos clusters
    for(int i = 0; i < numCluster; i++){
      //verifica se a distancia do cluster1 é menor que a o cluster2
      if(cluster1.getDist(i) > cluster2.getDist(i)){
        clusters.ElementAt(clusteId1).setDistId(i,cluster2.getDist(i));
        
        clusters.ElementAt(i).setDistId(clusteId1, cluster2.getDist(i));
      }
    }
    
    //for que percorre o cluster2 e passa todos os pontos dele para cluster1
    for(int i = 0; i < cluster2.getPontos().Count; i++){
      //cluster1 recebe os pontos de cluster2
      cluster1.addPonto(cluster2.getPonto(i));
    }
    
    //for que percorre todos os clusters e remove a distancia dele para o cluster de id clusterId2, pois esse cluster será removido apos a junção
    for(int i = 0; i < numCluster; i++){
      //remove as distancias dos clusters até cluster2
      clusters.ElementAt(i).removeDist(clusterId2);
    }
    //remove o cluster de id clusterId2
    clusters.RemoveAt(clusterId2);
    //diminui o total de clusters
    numCluster -= 1;
  }

  //função que retorna um cluster
  public ClusterSL getCluster(int idCluster){
    return clusters.ElementAt(idCluster);
  }
}