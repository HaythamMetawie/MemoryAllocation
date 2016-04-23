using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryAllocation
{   
 
    class memoryitem
    {
        public string type;
        public int size;
        public int starting_add;
        public memoryitem(){}
        public void setsize(int s) { size = s; }
        public int getsize() { return size; }
        public int getstratingadd() { return starting_add; }
        public void setstarting_add(int address) { starting_add = address; }

        public virtual string getname() { return ""; }
    }
    class process:memoryitem
    { 
        private string name;
        //private int allocated_space;
        public process(string n, int s) { name = n; size = s; type = "p"; }
        public override string getname() { return name; }
        
    };

    class hole:memoryitem
    {
            public hole(int add, int s) { size = s; starting_add = add; type = "h"; }
            
    };
    class methodology
    {
        public static void concatenate(List<memoryitem> L)
        {
            List<memoryitem> temp = L;
            temp.OrderBy(add =>add.getstratingadd());
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].type == "h")
                {
                    int add1 = temp[i].getstratingadd();
                    int size1 = temp[i].getsize();
                    int add2 = temp[i + 1].getstratingadd();
                    int size2 = temp[i + 1].getsize();
                    if (add1 + size1 == add2)
                    {
                        temp[i].setsize(size1 + size2);
                        temp.Remove(temp[i + 1]);
                        L = temp;
                    }
                }
            }
        }
        public static List<memoryitem> memory = new List<memoryitem>();
        public static List<memoryitem> allprocess = new List<memoryitem>();
        
        public void inserthole(hole hole){
            // check holes 
            memory.Add(hole);
        }
        public static List<memoryitem> FirstFit(process newprocess)
        {
            process nprocess = newprocess;

            foreach (var item in memory)
            {
                if (item.type == "h")
                {
                    if (item.size > newprocess.size)
                    {   
                        //get el starting address w el size  
                        int startaddress = item.getstratingadd();
                        int size = item.getsize();
                        //add new process in pos (starting address) 
                        nprocess.setstarting_add(startaddress);
                        memory.Add(nprocess);
                        //add new hole in pos start address+size of process its size equals ( old hole size - new process size)
                        hole nhole =new hole((startaddress+nprocess.getsize()),(size-nprocess.getsize()));
                        memory.Add(nhole);
                        //delete el hole el adema 
                        memory.Remove(item);
                        allprocess.Add(nprocess);    
                        break;
                    }
                    
                }

            }
            
            return memory;
        }

        public static List<memoryitem> BestFit(process newprocess)
        {
            process nprocess = newprocess;
            List<memoryitem> temp = memory;
            temp.OrderBy(a => a.getsize());
            foreach (var item in temp)
            {
                if (item.type == "h")
                {   
                    if (item.size > newprocess.size)
                    {
                        //get el starting address w el size  
                        int startaddress = item.getstratingadd();
                        int size = item.getsize();
                        //add new process in pos (starting address) 
                        nprocess.setstarting_add(startaddress);
                        memory.Add(nprocess);
                        //add new hole in pos start address+size of process its size equals ( old hole size - new process size)
                        hole nhole = new hole((startaddress + nprocess.getsize()), (size - nprocess.getsize()));
                        memory.Add(nhole);
                        //delete el hole el adema 
                        memory.Remove(item);
                        allprocess.Add(nprocess);
                        break;
                    }

                }

            }

            return memory;
        }

        public static List<memoryitem> WorstFit(process newprocess)
        {
            process nprocess = newprocess;
            List<memoryitem> temp = memory;
            temp.OrderByDescending(d => d.getsize());
            foreach (var item in temp)
            {
                if (item.type == "h")
                {
                    if (item.size > newprocess.size)
                    {
                        //get el starting address w el size  
                        int startaddress = item.getstratingadd();
                        int size = item.getsize();
                        //add new process in pos (starting address) 
                        nprocess.setstarting_add(startaddress);
                        memory.Add(nprocess);
                        //add new hole in pos start address+size of process its size equals ( old hole size - new process size)
                        hole nhole = new hole((startaddress + nprocess.getsize()), (size - nprocess.getsize()));
                        memory.Add(nhole);
                        //delete el hole el adema 
                        memory.Remove(item);
                        allprocess.Add(nprocess);
                        break;
                    }

                }

            }

            return memory;
        }
        public static List<memoryitem> deallocate(string processname)
        {
            string name = processname;
            
            foreach (var item in memory)
            {
                if (item.type == "p" && item.getname()==name)
                {
                    int size =item.getsize();
                    int address= item.getstratingadd();
                    memory.Remove(item);
                    allprocess.Remove(item);
                    hole nhole =new hole((address),(size));
                    memory.Add(nhole);
                    
                    //call the concatenation function
                    concatenate(memory);

                    break;
                }

            }
            
            return memory;
        }
    };
     
    
    class Program
    {
        static void Main(string[] args)
        { 
            hole h = new hole(1220 , 100);
            process p = new process("p1", 50);
            LinkedList<memoryitem> x = new LinkedList<memoryitem>();
            x.AddLast(h);
            x.AddLast(p);
            x.Remove(p);
            Console.WriteLine("Enter no of processes: ");
            List<string> list = new List<string>();
            string xx = Console.ReadLine();
            int xy = Convert.ToInt32(xx);
            //Console.Write(xx);

            for (int i = 0; i < xy; i++)
            {
                string f = Console.ReadLine();
                list.Add(f);
            }
            for (int i = 0; i < xy; i++)
            {
                Console.WriteLine(list.ElementAt(i));
            }
        }
    }
}
