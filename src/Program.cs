namespace DeadLockMalefico;

public static class Program
{
    public static async Task Main()
    {
        var recursoA = new Recurso();
        var recursoB = new Recurso();

        var task1 = Task.Run(() =>
        {
            Console.WriteLine("Task 1: Dando lock no recurso A para que seja possível manipulá-lo");
            lock (recursoA)
            {
                recursoA.SomaUm();
                Console.WriteLine("Task 1: Lock no recurso A efetuado. Agora tentando dar lock no recurso B");
                lock (recursoB)
                {
                    Console.WriteLine("Task 2: Recursos A e B estão locados, nunca vai chegar aqui");
                    recursoB.SubtraiUm();
                }
            }
        });

        var task2 = Task.Run(() =>
        {
            Console.WriteLine("Task 2: Dando lock no recurso B para que seja possível manipulá-lo");
            lock (recursoB)
            {
                recursoB.SomaUm();
                Console.WriteLine("Task 2: Lock no recurso B efetuado. Agora tentando dar lock no recurso A");
                lock (recursoA)
                {
                    Console.WriteLine("Task 2: Recursos B e A estão locados, nunca vai chegar aqui");
                    recursoA.SubtraiUm();
                }
            }
        });

        await task1;
        await task2;
        
        // Daqui pra baixo nada acontecerá, pois as tasks nunca vão terminar
        
        Console.WriteLine("Se tudo der certo e nada der errado, essa mensagem nunca será exibida");
    }
}

public class Recurso
{
    private int _total = 0;
    public int Total => _total;
    public void SomaUm()
    {
        _total++;
        Thread.Sleep(120); // simulação de um processo externo demorado
    }
    
    public void SubtraiUm()
    {
        _total--;
        Thread.Sleep(120); // simulação de um processo externo demorado
    }
}
