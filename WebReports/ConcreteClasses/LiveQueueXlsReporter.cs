using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WebReports.LiveQueueReport;
using WebReports.Abstractions;

namespace WebReports.ConcreteClasses
{
    public class LiveQueueXlsReporter:AbstractReporter
    {
        private XSSFWorkbook _book;
        private ISheet _sheet;
        private IRow _row;
        private ICell _cell;
        private Task _task;
        
        public Task CreateReport(List<Queue> queueInfo)
        {
            return _task = new Task(() =>
            {
                var filePath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\LiveQueueReport\table.xlsx";

                using (
                var file =
                    new FileStream(filePath,
                        FileMode.Open, FileAccess.Read))
                {
                    _book = new XSSFWorkbook(file);
                }

                _sheet = _book.GetSheetAt(0);

                var cellNum = 0;
                foreach (var queue in queueInfo)
                {
                    switch (queue.Department)
                    {
                        case "Чебаркуль":
                            cellNum = 1;
                            break;
                        case "Троицк":
                            cellNum = 2;
                            break;
                        case "Коркино":
                            cellNum = 3;
                            break;
                        case "Златоуст":
                            cellNum = 4;
                            break;
                        case "Кыштым":
                            cellNum = 5;
                            break;
                        case "Миасс":
                            cellNum = 6;
                            break;
                        case "Сосновка":
                            cellNum = 7;
                            break;
                        case "Магнитогорск":
                            cellNum = 8;
                            break;
                        //case "Челябинск":
                        //    _cell = _row.GetCell(7);
                        //    break;
                    }

                    foreach (var regSubService in queue.RegSubServices)
                    {
                        switch (regSubService.Code)
                        {
                            case "10002619923": //Прекращение регистрации ТС
                                SetServiceStateValue(2, cellNum, regSubService.State);
                                break;
                            case "10000466914":
                                //Регистрация автомототранспортного средства ранее не зарегистрированного в ГИБДД
                                SetServiceStateValue(3, cellNum, regSubService.State);
                                break;
                            case "10002619724": //Регистрация автомототранспортных средств на ограниченный срок
                                SetServiceStateValue(4, cellNum, regSubService.State);
                                break;
                            case "10000592270": //Восстановление регистрации автомототранспортного средства
                                SetServiceStateValue(5, cellNum, regSubService.State);
                                break;
                            case "10000593889": //Изменение регистрационных данных АМТС
                                SetServiceStateValue(6, cellNum, regSubService.State);
                                break;
                            case "10000593393":
                                //Изменение регистрационных данных, связанных с изменением регистрационных данных собственника ТС
                                SetServiceStateValue(7, cellNum, regSubService.State);
                                break;
                            case "10000593682":
                                //Изменение регистрационных данных транспортных средств, связанных с выдачей свидетельств о регистрации, ПТС, регистрационных знаков транспортных средств, взамен утраченных, непригодных для пользования
                                SetServiceStateValue(8, cellNum, regSubService.State);
                                break;
                            case "10002619870":
                                //Изменение регистрационных данных о собственнике (владельце) транспортного средства
                                SetServiceStateValue(9, cellNum, regSubService.State);
                                break;
                            case "10000594794":
                                //Снятие с регистрационного учета автомототранспортного средства в связи с утилизацией
                                SetServiceStateValue(10, cellNum, regSubService.State);
                                break;
                            case "10000595058":
                                //Снятие с регистрационного учета автомототранспортного средства в связи с вывозом за пределы Российской Федерации на постоянное пребывание
                                SetServiceStateValue(11, cellNum, regSubService.State);
                                break;
                        }
                    }

                    foreach (var examSubService in queue.ExamSubServices)
                    {
                        switch (examSubService.Code)
                        {
                            case "10000467319": //Получение международного водительского удостоверения
                                SetServiceStateValue(13, cellNum, examSubService.State);
                                break;
                            case "10000467506":
                                //Получение водительского удостоверения после прохождения профессиональной подготовки (переподготовки)
                                SetServiceStateValue(14, cellNum, examSubService.State);
                                break;
                            case "10000467646":
                                //Замена водительского удостоверения в связи с истечением срока его действия
                                SetServiceStateValue(15, cellNum, examSubService.State);
                                break;
                            case "317348568":
                                //Замена водительского удостоверения в случае изменения персональных данных его владельца, пришедшего в негодность и в случае его утраты (хищения)
                                SetServiceStateValue(16, cellNum, examSubService.State);
                                break;
                            case "317349234": //Получение российского национального водительского удостоверения
                                SetServiceStateValue(17, cellNum, examSubService.State);
                                break;
                        }
                    }
                }

                using (var file = new FileStream($@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\LiveQueueReport\out\{DateTime.Now.ToString("dd MMMM yyyy, ddd")}.xlsx",
                    FileMode.Create, FileAccess.Write))
                {
                    _book.Write(file);
                }
            });
        }

        private void SetServiceStateValue(int rowNum, int cellNum, string value)
        {
            _row = _sheet.GetRow(rowNum);
            _cell = _row.GetCell(cellNum);
            _cell.SetCellValue(value);
        }
    }
}