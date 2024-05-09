import {Pipe, PipeTransform} from '@angular/core';
import {constant} from 'case';

@Pipe({name: 'snakecase', standalone: true})
export class SnakeCasePipe implements PipeTransform {
  transform(value?: string): string {
    return constant(value ?? '');
  }
}
