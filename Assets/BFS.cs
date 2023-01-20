using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS : MonoBehaviour
{
    // Start is called before the first frame update
    private int bestLength;
    //je cree une paire de liste pour stocker chaque valeur et le chemin parcouru
    Queue<KeyValuePair<List<Vector2Int>, Vector2Int>> integer_stack = null;
    
    private List<Vector2Int> listcheckoutLength = null;
    bool[,] tarray;
    Vector2Int endvalue;
    Vector2Int beginvalue;
    public static BFS dfs;
    //tous les côtés
    /*int[] dx = { -1, 1, 0, 0, -1, -1, 1, 1 };
    int[] dy = { 0, 0, -1, 1, -1, 1, -1, 1 };*/
    int[] dx = { -1, 1, 0, 0 };
    int[] dy = { 0, 0, -1, 1 };

    List<(int x, int y)> listcheckoutLength_;

    int rows = 100;
    int cols = 100;


    Vector2Int v = new Vector2Int(10, 10);
    Vector2Int v2 = new Vector2Int(20,1);
    bool isCheckifDFS = false;


    void Start()
    {
        if (isCheckifDFS == false)
        {
            Debug.Log("message ");
            isCheckifDFS = true;

            checkupDfs(v, v2);
            List<Vector2Int> path = getPathsDfs();
            if (path.Count != 0)
            {
                string msg = "(";
                foreach (var item in path)
                {
                    msg += item.ToString() + ",";
                }
                msg += ")";
                Debug.Log(msg);
                listcheckoutLength.Clear();
            }
            else
            {
                //search another point
                Debug.Log("no arrived point finded");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

       

    }

    public List<Vector2Int> getPathsDfs()
    {
        return this.listcheckoutLength;
    }

    public  void checkupDfs(Vector2Int _begin, Vector2Int endvalue)
    {
        
        // path = new List<Vector2>();
        beginvalue = _begin;
        //Vector2 endvalue = _end;

        listcheckoutLength = new List<Vector2Int>();
        List<Vector2Int> lists = new List<Vector2Int>();
        lists.Add(_begin);
        integer_stack = null;
        integer_stack = new Queue<KeyValuePair<List<Vector2Int>, Vector2Int>>();
        integer_stack.Enqueue(new KeyValuePair<List<Vector2Int>, Vector2Int>(lists, _begin));

        tarray = new bool[rows, cols];
        //create array for checking if node has been visited
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                tarray[i, j] = false;
            }

        }

        DFSmethods(endvalue);

        //*********INSERT to _path (pNodes)***********

        //si cette liste qui contient le chemin le plus court n'est pas vide
        if (listcheckoutLength != null)
        {
            bestLength = listcheckoutLength.Count;

        }
        else
        {
            bestLength = int.MaxValue;
        }


        
        lists.Clear();




    }

    bool valid(int x, int y)
    {
        return (x >= 0 && x < rows && y >= 0 && y < cols);
    }


    public void DFSmethods(Vector2Int endvalue)
    {
      while (integer_stack.Count != 0 && tarray[(int)endvalue.x, (int)endvalue.y] == false) { 
            //je cree une paire  temporaire 
            //KeyValuePair<List<Vector2>, Vector2> tmp = integer_stack.Peek();//on recupere la derniere paire d'elements du stack (qui est le Top)
            //top element in the stack (warning: the top element is the last element added in stack)

        KeyValuePair<List<Vector2Int>, Vector2Int> tmp = integer_stack.Dequeue(); // remove the top element
                                                                                  //top element in the stack (warning: the top element is the last element added in stack)
        Vector2Int v = tmp.Value;
        Debug.Log(integer_stack.Count);                   //Debug.Log(v.x.ToString() + ' ' + v.y.ToString());


        bool find = tarray[(int)v.x, (int)v.y];
        if (find == false)
        {
            //si la valeur v correspond au sommet _end, 
            tarray[(int)v.x, (int)v.y] = true;
            if ((int)v.x == (int)endvalue.x && (int)v.y == (int)endvalue.y)
            {

                //si la liste n'est pas vide
                if (listcheckoutLength.Count != 0)
                {

                    //on compare la longueur du chemin actuel à la longueur du chemin se trouvant dans la list listcheckoutLength

                    if (tmp.Key.Count < listcheckoutLength.Count)
                    {
                        //on fait un copy constructeur
                        listcheckoutLength = new List<Vector2Int>(tmp.Key);
                        bestLength = tmp.Key.Count;

                    }
                }
                else
                {//si la liste listcheckout est vide

                    listcheckoutLength = new List<Vector2Int>(tmp.Key);


                }
                //la valeur du end dans le tableau booleen  est mis  à false
                //tarray[(int)endvalue.x, (int)endvalue.y] = false;
            }
            else
            {
                int valuex = (int)v.x;
                int valuey = (int)v.y;
                // on ajoute tous les somments suivants de v dans le stack

                for (int j = 0; j < dx.Length; ++j)
                {

                    int next_x = valuex + dx[j];
                    int next_y = valuey + dy[j];
                    //if (valid(next_x, next_y) )
                    if (valid(next_x, next_y) && (Grid.GetPositionMatrix(next_x, next_y) == false && (tarray[next_x, next_y] == false)))
                    {


                        //copie de la liste
                        //listcheckoutLength = new List<Vector2>(tmp.Key);
                        List<Vector2Int> tmpCopie = new List<Vector2Int>(tmp.Key);
                            //List<Vector2> tmpCopie = new List<Vector2>();
                            //tmpCopie = tmp.Key;

                            //on ajoute l'element j dans la liste
                         Vector2Int vq = new Vector2Int(next_x, next_y);

                        tmpCopie.Add(vq);
                        //KeyValuePair<List<Vector2>, Vector2> tmp2 = new KeyValuePair<List<Vector2>, Vector2>(tmpCopie, vq);
                        // on ajoute cette paire dans le stack
                        //integer_stack.Push(tmp2);
                        integer_stack.Enqueue(new KeyValuePair<List<Vector2Int>, Vector2Int>(tmpCopie, vq));
                        tmpCopie = null;





                    }
                }


            }

        }
        tmp.Key.Clear();

        /* if (integer_stack.Count != 0 && tarray[(int)endvalue.x, (int)endvalue.y] ==false)
         {
           return  DFSmethods(endvalue);
         }
         else
         {
             Debug.Log("stack vide");
             return 0;
         }*/
    }
    }
}

