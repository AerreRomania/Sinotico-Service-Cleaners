using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using  System.Data;
using wcfcleaners.Model;

namespace wcfcleaners
    {
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
        {
        /// <summary>
        /// Gets alarmed data.
        /// </summary>
        /// <returns>data table</returns>
        [OperationContract]
        DataTable GetAlmTable(string tablename);
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>data table</returns>
        [OperationContract]
        DataTable GetGeneralTable();
        /// <summary>
        /// Gets the data using data contract.
        /// </summary>
        /// <param name="composite">The composite.</param>
        /// <returns></returns>
        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        // TODO: Add your service operations here
        [OperationContract]
        string[] GetOperatorName(string code);
        /// <summary>
        /// Gets the progressive number.
        /// </summary>
        /// <param name="evDate">The ev date.</param>
        /// <param name="cShift">The c shift.</param>
        /// <param name="mac">The mac.</param>
        /// <returns></returns>
        [OperationContract]
        int GetProgressiveNumber(DateTime evDate, string cShift, int mac, string operatorx, string type, string tablename);
        /// <summary>
        /// Inserts the new operation.
        /// </summary>
        /// <param name="evDate">The ev date.</param>
        /// <param name="cShift">The c shift.</param>
        /// <param name="operatorName">Name of the operator.</param>
        /// <param name="cStart">The c start.</param>
        /// <param name="cEnd">The c end.</param>
        /// <param name="machine">The machine.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="cType">Type of the c.</param>
        /// <param name="note">The note.</param>
        /// <param name="dateLoad">The date load.</param>
        /// <param name="progNum">The prog number.</param>
        /// <param name="tempoMin">The tempo minimum.</param>
        /// <param name="ptPrec">The pt prec.</param>
        [OperationContract]
        void InsertNewOperation(DateTime evDate, string cShift, string operatorName, DateTime cStart, DateTime cEnd,
            int machine, string reason, string cType, string note, DateTime dateLoad, int progNum, int tempoMin, string ptPrec);
        /// <summary>
        /// Updates the operation.
        /// </summary>
        /// <param name="endclean"></param>
        /// <param name="id"></param>
        [OperationContract]
        void UpdateOperation(DateTime endclean, long id);
        /// <summary>
        /// Updates the operation with additional note.
        /// </summary>
        /// <param name="evDate">The ev date.</param>
        /// <param name="cShift">The c shift.</param>
        /// <param name="progNum">The prog number.</param>
        [OperationContract]
        void UpdateOperation1(DateTime endclean, long id, string note);

        [OperationContract]
        void UpdateOperation2(DateTime endclean, long id, int timeStamp);

        [OperationContract]
        void UpdateOperation3(DateTime endclean, long id, string note, int timeStamp);


        [OperationContract]
            void UpdateOperationManually(int id, string note);

            [OperationContract]
            long GetJobId(string table);

        [OperationContract]
        double GetTimeStampInMM(DateTime start, DateTime end);
        /// <summary>
        /// Gets the shift.
        /// </summary>
        [OperationContract]
        string GetShift();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract] bool HasError();
        /// <summary>
        /// Activates alarm status
        /// </summary>
        /// <param name="id">Unique job id</param>
        [OperationContract]
        void ActivateAlarm(long id);
        /// <summary>
        /// Activates alarm status
        /// </summary>
        /// <param name="id">Unique job id</param>
        /// <param name="note">Overload argumet</param>
        [OperationContract]
        void ActivateAlarmNote(long id, string note);
        /// <summary>
        /// Deactivates alarm status
        /// </summary>
        /// <param name="id">Unique job id</param>
        [OperationContract]
        void DeactivateAlarm(long id, string tablename, string user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oper_id"></param>
        /// <param name="evDate"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        [OperationContract]
        DataTable MachineTable(string oper_id, DateTime evDate, string shift);

        /// <summary>
        /// Gets color, size, components and article from database for inserted machine number
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        [OperationContract]
        DataTable LoadMachineInformations(int mac);
        /// <summary>
        /// Inserts data in datatable
        /// </summary>
        /// <param name="macc"></param>
        /// <param name="comm"></param>
        /// <param name="art"></param>
        /// <param name="comp"></param>
        /// <param name="tag"></param>
        /// <param name="col"></param>
        /// <param name="cotta"></param>
        /// <param name="prog"></param>
        /// <param name="noteG"></param>
        /// <param name="noteA"></param>
        /// <param name="ag"></param>
        /// <param name="rang"></param>
        /// <param name="lung"></param>
        /// <param name="lang"></param>
        /// <param name="elastic"></param>
        /// <param name="alt"></param>
        /// <param name="cat"></param>
        /// <param name="sx"></param>
        /// <param name="cart"></param>
        /// <param name="infil"></param>
        /// <param name="cottaFil"></param>
        /// <param name="startEv"></param>
        /// <param name="endEv"></param>
        /// <param name="operatId"></param>
        /// <param name="shift"></param>
        [OperationContract]
        void InsertQualityRecord(int macc, string comm, string art, string comp, string tag, string col, string cotta, int prog,
                                        string noteA, string ag, string rang, string lung, string lang, string elastic,
                                        string alt, string cat, string sx, string cart, string infil, string cottaFil,
                                        DateTime startEv, DateTime endEv, int operatId, string shift, string esi, string fullname);

        [OperationContract]
        DataSet GetEmailInfo();

        /// <summary>
        /// Insert operators job info. Successfull insertion will return inserted row, otherwise return null. 
        /// </summary>
        /// <param name="job">Contains information about operators job</param>
        [OperationContract]
        OperatorsJob StartOperatorsJob(string FullName, string Line);

        /// <summary>
        /// Sends operators login request. If operator with sent pin exist it will return information about operator, otherwise null.
        /// </summary>
        /// /// <param name="pin">Operators login pin code</param>
        [OperationContract]
        Operator LoginOperator(string pin);

        /// <summary>
        /// Update operators started job.
        /// </summary>
        /// /// <param name="jobId">Started operators job id</param>
        [OperationContract]
        void EndOperatorsJob(long jobId);

        /// <summary>
        /// Returns machines for sent line if line exist, otherwise returns null.
        /// </summary>
        /// /// <param name="line">The line for which operator is in charge</param>
        /// /// <param name="shift">Current shift</param>
        [OperationContract]
        List<Machine> GetLineMachines(string line);
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
        {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
            {
            get { return boolValue; }
            set { boolValue = value; }
            }

        [DataMember]
        public string StringValue
            {
            get { return stringValue; }
            set { stringValue = value; }
            }
        }

}
