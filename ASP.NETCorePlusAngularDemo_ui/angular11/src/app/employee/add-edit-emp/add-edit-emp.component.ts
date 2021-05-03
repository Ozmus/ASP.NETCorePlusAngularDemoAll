import { Component, OnInit, Input } from '@angular/core';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-add-edit-emp',
  templateUrl: './add-edit-emp.component.html',
  styleUrls: ['./add-edit-emp.component.css']
})
export class AddEditEmpComponent implements OnInit {

  constructor(private service: SharedService) { }

  @Input() emp: any;
  EmployeeId: string = "";
  EmployeeName: string = "";
  DepartmentId: string = "";
  DateOfJoin: string = "";
  PhotoFileName: string = "";

  ngOnInit(): void {
    this.EmployeeId = this.emp.EmployeeId;
    this.EmployeeName = this.emp.EmployeeName;
    this.DepartmentId = this.emp.DepartmentId;
    this.DateOfJoin = this.emp.DateOfJoin;
    this.PhotoFileName = this.emp.PhotoFileName;
  }

  addEmployee(){
    var val = {EmployeeId: this.EmployeeId,
               EmployeeName: this.EmployeeName};
    this.service.addEmployee(val).subscribe(res => {
      alert(res.toString())
    });
  }

  updateEmployee(){
    var val = {EmployeeId: this.EmployeeId,
               EmployeeName: this.EmployeeName,
               DepartmentId: this.DepartmentId,
               DateOfJoin: this.DateOfJoin,
               PhotoFileName: this.PhotoFileName
             };
    this.service.updateEmployee(val).subscribe(res => {
      alert(res.toString())
    });
  }
}

