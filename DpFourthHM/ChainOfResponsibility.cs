using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpFourthHM
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);

        object Handle(object request);
    }

    abstract class AbstractHandler : IHandler
    {
        private IHandler _nextHandler;

        public IHandler SetNext(IHandler handler)
        {
            this._nextHandler = handler;
            return handler;
        }

        public virtual object Handle(object request)
        {
            if (this._nextHandler != null)
            {
                return this._nextHandler.Handle(request);
            }
            else
            {
                return null;
            }
        }
    }

    class Mist200 : AbstractHandler
    {
        public override object Handle(object request)
        {
            if ((request as string) == "400")
            {
                return $" Это = Проблемы при попытки вывести запрос {request.ToString()}.\n";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

    class Mist400 : AbstractHandler
    {
        public override object Handle(object request)
        {
            if (request.ToString() == "200")
            {
                return $" Это = Запрос обработан успешно  {request.ToString()}.\n";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

    class Mist404 : AbstractHandler
    {
        public override object Handle(object request)
        {
            if (request.ToString() == "404")
            {
                return $" Это = Проблемы при попытки выести запрос {request.ToString()}.\n";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

    class Mist501 : AbstractHandler
    {
        public override object Handle(object request)
        {
            if (request.ToString() == "501")
            {
                return $" Это = Ошибка сервера {request.ToString()}.\n";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

    class Client
    {
        // Обычно клиентский код приспособлен для работы с единственным
        // обработчиком. В большинстве случаев клиенту даже неизвестно, что этот
        // обработчик является частью цепочки.
        public static void ClientCode(AbstractHandler handler)
        {
            foreach (var MisTake in new List<string> { "200", "400", "404", "501" })
            {
                Console.WriteLine($"Client: What is it {MisTake}?");

                var result = handler.Handle(MisTake);

                if (result != null)
                {
                    Console.Write($"   {result}");
                }
                else
                {
                    Console.WriteLine($"   {MisTake} was left untouched.");
                }
            }
        }
    }

    class ChainOfResponsibility
    {
        static void Main(string[] args)
        {
            // Другая часть клиентского кода создает саму цепочку.
            var mist200 = new Mist200();
            var mist400 = new Mist400();
            var mist404 = new Mist404();
            var mist501 = new Mist501();

            mist200.SetNext(mist400).SetNext(mist404).SetNext(mist501);

            // Клиент должен иметь возможность отправлять запрос любому
            // обработчику, а не только первому в цепочке.
            Console.WriteLine("HTTP: 200,400,404,501\n");
            Client.ClientCode(mist200);
            Console.WriteLine();

        }
    }
}

