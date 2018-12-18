using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Entities
{
    [DataContract]
    public class ResultValue<T>
    {
        public ResultValue(T result, Exception ex)
        {
            this.Ex = ex;
            this.Result = result; 
        }
        public ResultValue(Exception e) : this(default(T), e) { }

        public ResultValue(T result)
            : this(result, null)
        {}
        
        public ResultValue(T result, bool isAuthenticated):this(result, null)
        {
            this.isAuthenticated = isAuthenticated;
        }
        [DataMember]
        private bool isAuthenticated = true;
        [DataMember]
        private T Result {get; set;}
        [DataMember]
        private Exception Ex {get; set;}
        

        public T TryGetResult()
        {
            if (Ex != null || !isAuthenticated)
                return default(T);
            else return Result;
        }
        /// <summary>
        /// Zwraca wartość otrzymaną przez WebService. Rzuca wyjątek jeśli wystąpił w WebService
        /// </summary>
        /// <returns></returns>
        public T GetResult()
        {
            if (Ex != null)
                throw new Exception(Ex.Message, Ex);
            else if (!isAuthenticated)
                throw new Exception("Nie jesteś zalogowany lub brak uprawnień");
                
            return Result;
	
        }
    }
}