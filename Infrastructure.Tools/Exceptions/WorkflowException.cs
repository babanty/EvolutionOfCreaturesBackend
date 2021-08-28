using System;

namespace Infrastructure.Tools.Exceptions
{
    public class WorkflowException : Exception
    {
        public WorkflowException(string stateTypes, string stateFrom, string stateTo) : base($"Переход из состояния {stateFrom} в состояние {stateTo} запрещен. Тип состояний {stateTypes}.")
        {

        }
    }
}
