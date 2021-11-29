using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

class Program {

  public static void Main (string[] args) {
    //estamos representado os valores dos objetos na forma de ponto
    //e em todo momento em que falamos de pontos estamos nos referindo aos objetos

    //transforma os dados dos 3 arquivos em vetor de pontos
    Ponto[] pontosS1 = lerArquivo("./datasets/c2ds1-2sp.txt");
    Ponto[] pontosS3 = lerArquivo("./datasets/c2ds3-2g.txt");
    Ponto[] pontosMk = lerArquivo("./datasets/monkey.txt");

    //verifica se o diretorio de saida existe
    if(!Directory.Exists("./resultados")){
      //se não existir cria o diretorio
      Directory.CreateDirectory("./resultados");
    }

    
    //chama o singlelink para cada arquivo
    singlelink(pontosS1, 2, 5, "./resultados/c2ds1-2spSL");
    singlelink(pontosS3, 2, 5, "./resultados/c2ds3-2gSL");
    singlelink(pontosMk, 5, 12, "./resultados/monkeySL");
    

    //chama o kmedias para os dados do aquivo c2ds1-2sp
    kmedias(pontosS1, 2, 80, "./resultados/c2ds1-2spKM2");
    kmedias(pontosS1, 3, 80, "./resultados/c2ds1-2spKM3");
    kmedias(pontosS1, 4, 80, "./resultados/c2ds1-2spKM4");
    kmedias(pontosS1, 5, 80,  "./resultados/c2ds1-2spKM5");

    //chama o kmedias para os dados do aquivo c2ds3-2g
    kmedias(pontosS3, 2, 60, "./resultados/c2ds3-2gKM2");
    kmedias(pontosS3, 3, 60, "./resultados/c2ds3-2gKM3");
    kmedias(pontosS3, 4, 60, "./resultados/c2ds3-2gKM4");
    kmedias(pontosS3, 5, 60,  "./resultados/c2ds3-2gKM5");

    //chama o kmedias para os dados do aquivo monkey
    kmedias(pontosMk, 5, 95, "./resultados/monkeyKM5");
    kmedias(pontosMk, 6, 95, "./resultados/monkeyKM6");
    kmedias(pontosMk, 7, 95,  "./resultados/monkeyKM7");
    kmedias(pontosMk, 8, 95, "./resultados/monkeyKM8");
    kmedias(pontosMk, 9, 95, "./resultados/monkeyKM9");
    kmedias(pontosMk, 10, 95,  "./resultados/monkeyKM10");
    kmedias(pontosMk, 11, 95, "./resultados/monkeyKM11");
    kmedias(pontosMk, 12, 95, "./resultados/monkeyKM12");

  }

  private static void singlelink(Ponto[] ponto, int kmin,  int kmax, string dir)
  {
    //separar objetos em cluster
    //distancia euclidiana
    //junta clusters com menor distância até chegar ao número de clusters desejados
    
    //variavel usada para garantir a presença do ponto ao passar double para string
    NumberFormatInfo nfi = new NumberFormatInfo();
    nfi.NumberDecimalSeparator = ".";
    
    //for que deleta os arquivos da execução anterior
    for(int k = kmin; k <= kmax;k++){
      if(File.Exists(dir+k+".txt")) File.Delete(dir+k+".txt");
      if(File.Exists(dir+k+"Dados.txt")) File.Delete(dir+k+"Dados.txt");
    }    

    //cria a particao para o singlelink
    ParticaoSL particao = new ParticaoSL(ponto.Length, ponto);
      
    //variavel que guarda o numero de cluster atual
    int numClust = particao.getNumCluster();

    //variavel que guardam quais cluster devem ser unidos após cada iteração
    int idCluster1 = 0, idCluster2 = 0;

    //while que repete o processo até que o numero de clusters atinga o kmin
    while(particao.getNumCluster() >= kmin){      
      //variavel que sera usada para salvar a menor distancia entre cluster encotrada
      double minDist = Double.PositiveInfinity;

      //for que percorre todos os clusters
      for(int i = 0; i < particao.getNumCluster(); i++){

        //pega o cluster atual(i)
        ClusterSL cluster = particao.getCluster(i);

        //for que percorre todos os clusters após o atual(i+1), ja que
        //a distancia de um cluster x até um y é igual a de y até x
        for(int j = i+1; j < particao.getNumCluster(); j++){
          //checa se a distancia entre os clusters é menor que a distancia encontrada anteriormente
          if(cluster.getDist(j) < minDist)
          {
            //se for menor salva a nova distancia
            minDist = cluster.getDist(j);
            //e salva o id dos dois clusters
            idCluster1 = i;
            idCluster2 = j;
          }
        }          
      }
      //junta o cluster de idCluter2 ao de idCluster1
      particao.juntarCluster(idCluster1, idCluster2);
      
      //pega o novo numero de cluster da classe
      numClust = particao.getNumCluster();
      //verifica se o numero de clusters está entre kmax e kmin, para impressao
      if(numClust <= kmax && numClust >= kmin)
      {
        //variavel que numera os cluster no arquivo
        int clusterNum = 1;
        //for que percorre todos os clusters
        for(int clusterID = 0; clusterID < particao.getNumCluster(); clusterID++){
          //variavel que guarda o cluster
          ClusterSL cluster = particao.getCluster(clusterID);

          //for que percorre todos os pontos do cluster
          for(int pontoID = 0; pontoID < cluster.getPontos().Count; pontoID++)
          {
            //comando que imprime o objeto atual e seu cluster no arquivo
            File.AppendAllText(dir+numClust+".txt", cluster.getPonto(pontoID).getNome() + " " + clusterNum + "\n");
        
            //arquivo para gerar o grafico
            //File.AppendAllText(dir+numClust+"Dados.txt",cluster.getPonto(pontoID).getNome() + "," + clusterNum + "," + cluster.getPonto(pontoID).getX().ToString(nfi) + "," + cluster.getPonto(pontoID).getY().ToString(nfi) +"\n");
            
          }
          //aumento o id do cluster no arquivo
          clusterNum += 1;
        }
      }
    }
  }

  private static void kmedias(Ponto[] ponto, int k, int parada, string dir)
  {
    //separa os pontos em clusters e considerando o ponto medio dos cluster
    //realoca pontos para os clusters de centros mais proximos

    //variavel usada para garantir a presença do ponto ao passar double para string
    NumberFormatInfo nfi = new NumberFormatInfo();
    nfi.NumberDecimalSeparator = ".";

    //cria a particao para o kmedias
    ParticaoKM particao = new ParticaoKM(ponto.Length, k, ponto);
    
    //variavel que salva as posições dos centroides
    Ponto[] centroid = new Ponto[k];

    //distancia usada no calculo da distancia euclidiana entre os pontos e os centroides
    double dist = 0;


    //variavel que salva o numero de clusters na partição
    int numClust = particao.getNumCluster();

    //variavel que determina o numero de loops executados
    int parar = 0;

    //boolean que para o algoritimo quando não ocorre nenhuma mudança nos clsuters
    bool parouMudanca = false;
    //while que executa o algoritimo em loop
    while(parar <= parada && parouMudanca == false){
      //marca o boolean de oarada como true, caso não mude ele finaliza o algoritimo
      parouMudanca = true;

      //for que passa todos os cluster para calcular o centroide
      for(int i = 0; i < numClust; i++){
        //calcula o centroide para cada cluster
        centroid[i] = particao.media(particao.getClusterList()[i]);
      }

      //for que percorre os clusters
      for(int clusterID = 0; clusterID < numClust; clusterID++){
        //variavel que guarda o cluster
        List<Ponto> cluster = particao.getClusterList()[clusterID];

        //for que percorre os pontos dentro do cluster atual
        for(int pontoID = 0; pontoID < cluster.Count; pontoID++){
          //variavel usada na comparação da menor distancia entre os pontos e os centroides
          double menor = Double.PositiveInfinity;

          //variavel que marca qual o cluster mais proximo do ponto
          int clusNumber = 0;

          //for que percorre os centroides para o calculo de distancia
          for(int controidID = 0; controidID < numClust; controidID++){
            //distancia euclidiana entre o ponto e o centroide atual
            dist = particao.DistanciaEuclidiana(cluster[pontoID], centroid[controidID]);

            //if que verifica se a distancia até o centroide atual é menor que a dos outros centroides
            if(dist < menor){
              //salva a nova menor distancia
              menor = dist;
              //salva o id do cluster desse centroide
              clusNumber = controidID;
            }
          }
          
          //se o id do cluster mais proximo é diferente do id do cluster que o ponto esta atualmente entao muda o ponto de cluster.
          if(clusNumber != clusterID){
            //troca o ponto de cluster
            particao.trocarPontoCluster(pontoID, clusterID, clusNumber);
            //como houve mudança então o algoritimo muda a boolean de parada para falso
            parouMudanca = false;
          }
        }
      }

      //adiciona mais um no contador de execuções  
      parar++;
    }

    //caso já exista algum arquivo anterior de resultados, deleta ele para escrever um novo
    if(File.Exists(dir+".txt")) File.Delete(dir+".txt");
    if(File.Exists(dir+"Dados.txt")) File.Delete(dir+"Dados.txt");

    for(int clusterID = 0; clusterID < numClust; clusterID++){
      //variavel que guarda o cluster
      List<Ponto> cluster = particao.getClusterList()[clusterID];
      
      //for que faz a impressao dos pontos do cluster no arquivo
      for(int pontoID = 0; pontoID < cluster.Count; pontoID++)
      {
        //imprime o objeto atual e cluster que ele pertence
        File.AppendAllText(dir+".txt", cluster[pontoID].getNome() + " " + (clusterID+1) + "\n");
        
        //arquivo para gerar o grafico
        //File.AppendAllText(dir+"Dados.txt", cluster[pontoID].getNome() + "," + (clusterID+1) + "," + cluster[pontoID].getX().ToString(nfi) + "," + cluster[pontoID].getY().ToString(nfi) +"\n");
      }
    }

  }

  private static Ponto[] lerArquivo(string entradas)
  {    
    //Le o arquivo e salva em linhas
    IEnumerable<string> linhas = File.ReadLines(entradas);

    //variavel que salva o numero de entradas
    int qtdentradas = linhas.Count();

    //variavel que salva a lista de pontos
    Ponto[] ponto = new Ponto[qtdentradas-1];

    //variavel que salva cada linha
    string linha;

    //for que percorre as linhas do arquivo
    for(int i = 1; i < qtdentradas; i++)
    {
      //variaveis que guardam o do objeto na linha
      double tempx = 0, tempy = 0;
      string tempName = "";

      //pega a linha atual
      linha = linhas.ElementAt(i);
      //usa os espaços para separar a linha em 3 partes(Nome, valor1 e valor2)
      string[] linhaPartes = linha.Split(null);

      //salva os dados do objeto nas variaveis temporarias
      tempName = linhaPartes[0];
      //CultureInfo é para garantir o funcionamento do double em todas as linguas
      tempx = double.Parse(linhaPartes[1], CultureInfo.InvariantCulture);
      tempy = double.Parse(linhaPartes[2], CultureInfo.InvariantCulture);
      
      //salva o ponto da linha atual no vetor de pontos
      ponto[i - 1] = new Ponto(tempx, tempy, tempName);
    }
    
    //retorna todos os pontos encontrados
    return ponto;  
  }
}

 