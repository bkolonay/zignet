import moment from 'moment';
import UtcDate from '../UtcDate';

it('gets date and time string', () => {
  let utcDate = new UtcDate("2018-03-13T19:11:27");
  expect(utcDate.getDateAndTimeString()).toEqual("03/13/2018 7:11:27 PM");
});

it('converts utc to local', () => {
  let utcDate = new UtcDate("2018-03-13T19:11:27");
  expect(utcDate.toLocal().getDateAndTimeString()).toEqual("03/13/2018 12:11:27 PM");
});

it('returns time from x', () => {
  let utcDate = new UtcDate("2018-03-13T19:11:27");
  expect(utcDate.getTimeFrom("2018-03-13T19:12:27")).toEqual("a minute");
});