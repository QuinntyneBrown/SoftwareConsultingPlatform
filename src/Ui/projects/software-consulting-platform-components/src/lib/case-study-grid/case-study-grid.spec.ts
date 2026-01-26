import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CaseStudyGrid } from './case-study-grid';

describe('CaseStudyGrid', () => {
  let component: CaseStudyGrid;
  let fixture: ComponentFixture<CaseStudyGrid>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CaseStudyGrid],
    }).compileComponents();

    fixture = TestBed.createComponent(CaseStudyGrid);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render component', () => {
    fixture.detectChanges();
    const element = fixture.nativeElement.querySelector('.sc-case-study-grid');
    expect(element).toBeTruthy();
  });
});
