using CRUD.Models;
using CRUD.Servises;

namespace CRUD
{
    public class Program
    {
        static void Main(string[] args)
        {
            /*Students student = new Students();
            student.fullName = "James Bond";
            student.grade = 4;
            student.avg_score = 4;

            Students student1 = new Students();
            student1.fullName = "Abdulla Hasanov";
            student1.grade = 5;
            student1.avg_score = 3;
            List<Students> students = new List<Students>() {student,student1 };*/


            /* Servises.CRUD.CreateTable();
             Servises.CRUD.InsertIntoTable(student);*/
            //Servises.CRUD.InsertSeveralStudents(students);
            // Servises.CRUD.GetAll();

            //  Servises.CRUD.GetById(1);
            // Servises.CRUD.Delete(1);

            /*            Students studentsForUpdate = new Students();
                        studentsForUpdate.fullName = "Updated student";
                        studentsForUpdate.grade = 1;
                        studentsForUpdate.avg_score = 3;
                        Servises.CRUD.UpdateStudent(studentsForUpdate, 3);*/

            Servises.CRUD.UpdateAnyColumn(3, "full_name", "Abdullajon");
        }
    }
}
