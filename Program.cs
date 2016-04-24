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
            public hole(int add=0, int s=0) { size = s; starting_add = add; type = "h"; }
            
    };
    class methodology
    {
        public static void concatenate(List<memoryitem> L)
        {
            List<memoryitem> temp = L;
            List<memoryitem> holes = new List<memoryitem>();
            hole h = new hole(); 
            foreach (var item in temp)
            {
                if (item.type == "h")
                {
                    holes.Add(item);
                }
            }
            holes.OrderBy(add=>add.getstratingadd());
            for (int i = 0; i < holes.Count-1; i++)
            {
                int size1 = holes[i].getsize();
                int size2 = holes[i + 1].getsize();
                int add1 = holes[i].getstratingadd();
                int add2 = holes[i + 1].getstratingadd();
                if (add1+size1 == add2)
                {
                    L.Remove(holes[i]);
                    L.Remove(holes[i + 1]);
                    h.setsize(size1 + size2);
                    h.setstarting_add(add1);
                    L.Add(h);
                }
            }
         
            
            
        }
        //public static List<memoryitem> memory = new List<memoryitem>();
        public static List<memoryitem> allprocess = new List<memoryitem>();
        
        public void inserthole(hole hole,List<memoryitem>memory){
            // check holes 
            memory.Add(hole);
        }
        public static List<memoryitem> FirstFit(process newprocess, List<memoryitem> memory)
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
                        hole nhole = new hole((startaddress + nprocess.getsize()), (size - nprocess.getsize()));
                        memory.Add(nhole);
                        //delete el hole el adema 
                        memory.Remove(item);
                        allprocess.Add(nprocess);
                        break;
                    }
                    //else Console.WriteLine("Process {0} is aded to waiting queue.", nprocess.getname());
                    
                }

            }
            
            return memory;
        }

        public static List<memoryitem> BestFit(process newprocess,List<memoryitem>memory)
        {
            process nprocess = newprocess;
            List<memoryitem> temp = memory;
            
            foreach (var item in temp)
            {
                temp.OrderBy(a => a.getsize());

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

        public static List<memoryitem> WorstFit(process newprocess,List<memoryitem>memory)
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
        public static List<memoryitem> deallocate(string processname,List<memoryitem>memory)
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
            hole h1 = new hole(50 , 220);
            hole h2 = new hole(300, 100);
            hole h3 = new hole(560, 140);
            hole h4 = new hole(700, 80);
            process p1 = new process("p1", 150);
            process p2 = new process("p2", 130);
            process p3 = new process("p3", 270);
            process p4 = new process("p4", 50);
            process p5 = new process("p5", 180);
            process p6 = new process("p6", 90);

            List<memoryitem> x = new List<memoryitem>();
            x.Add(h1); x.Add(h2); x.Add(h3); x.Add(h4); x.Add(p1); x.Add(p2); x.Add(p3); x.Add(p4); x.Add(p5); x.Add(p6);

            MemoryAllocation.methodology.FirstFit(p1, x);
            MemoryAllocation.methodology.FirstFit(p2, x);
            MemoryAllocation.methodology.FirstFit(p3, x);
            MemoryAllocation.methodology.FirstFit(p4, x);
            MemoryAllocation.methodology.FirstFit(p5, x);
            MemoryAllocation.methodology.FirstFit(p6, x);
            MemoryAllocation.methodology.deallocate("p4", x);

            //MemoryAllocation.methodology.BestFit(p1, x);
            //MemoryAllocation.methodology.BestFit(p2, x);
            //MemoryAllocation.methodology.BestFit(p3, x);
            //MemoryAllocation.methodology.BestFit(p4, x);
            //MemoryAllocation.methodology.BestFit(p5, x);
            //MemoryAllocation.methodology.BestFit(p6, x);
           
            foreach (var item in x)
            {
                Console.Write(item.getstratingadd());
                Console.WriteLine();
                Console.Write(item.getsize());
                Console.WriteLine();
            
            }

        }
    }
}
