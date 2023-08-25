import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css']
})
export class ServerErrorComponent implements OnInit {
  error:any;
  //We need to define router in constructer and ngOnInit() is already late.
  //We can only access it when we are in constructor. 
  constructor(private router:Router){
    const navigation = router.getCurrentNavigation();
    this.error = navigation?.extras?.state?.['error'];
  }
  ngOnInit(): void {
    
  }

}
