import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ExpirationDateService } from 'src/app/services/expiration-date.service';
import { AlertifyService } from 'src/app/services/alertify.service';
import { MatDialog } from '@angular/material/dialog';
import { ExpirationDateAddComponent } from '../expiration-date-add/expiration-date-add.component';
import { ExpirationDate } from 'src/app/models/expiration-date/expiration-date.model';
import { PaginatedComponentBase } from 'src/app/shared/helpers/paginated-component-base';
import { ManageFiltersComponent } from 'src/app/shared/manage-filters/manage-filters.component';

@Component({
  selector: 'app-expiration-dates-list',
  templateUrl: './expiration-dates-list.component.html',
  styleUrls: ['./expiration-dates-list.component.css'],
})
export class ExpirationDatesListComponent
  extends PaginatedComponentBase<ExpirationDate>
  implements OnInit {
  expirationDates: ExpirationDate[];
  constructor(
    private route: ActivatedRoute,
    private expirationDateService: ExpirationDateService,
    private alertify: AlertifyService,
    private dialog: MatDialog
  ) {
    super();
  }

  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.expirationDates = this.initializePagination(data.list);
    });
  }

  loadExpirationDates(event: any) {
    if (event) {
      this.currentPage = event.page;
    }
    this.expirationDateService
      .getAllPaginated(this.currentPage, this.itemsPerPage)
      .subscribe(
        (data) => {
          this.expirationDates = this.initializePagination(data);
        },
        (error) => {
          this.alertify.error(error);
        }
      );
  }

  showFilterDialog() {
    const filtersDialogRef = this.dialog.open(ManageFiltersComponent, {});

    filtersDialogRef.afterClosed().subscribe((result) => {
      console.log(result);
    });
  }

  showAddExpDateDialog() {
    const expDateAddDialog = this.dialog.open(ExpirationDateAddComponent, {});

    expDateAddDialog.afterClosed().subscribe((result) => {
      if (result) {
        this.loadExpirationDates(this.currentPage);
      }
    });
  }
}
