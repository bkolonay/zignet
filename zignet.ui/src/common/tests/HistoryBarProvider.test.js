import moment from 'moment';
import HistoryBarProvider from '../HistoryBarProvider'

// getErrorDivAttributes()
it('gets a failure duration with a start and end time', () => {
  let now = moment("2018-05-07T01:00:00-07:00"); // 5/7/2018 1am
  var failureStart = moment("2018-05-06T03:00:00-07:00"); // 5/6/2018 3am
  var failureEnd = moment("2018-05-06T04:00:00-07:00"); // 5/6/2018 4am
  let testFailureDuration = {
    FailureStart: failureStart.utc().format(),
    FailureEnd: failureEnd.utc().format()
  };

  let historyBarProvider = new HistoryBarProvider();
  let errorDivAttributes = historyBarProvider.getErrorDivAttributes(309, now, testFailureDuration);

  expect(errorDivAttributes.left).toEqual(25);
  expect(errorDivAttributes.width).toEqual(12);
});

it('gets a failure with a duration of 1 second', () => {
  let now = moment("2018-05-07T01:00:00-07:00"); // 5/7/2018 1am
  var failureStart = moment("2018-05-06T03:00:00-07:00"); // 5/6/2018 3am
  var failureEnd = moment("2018-05-06T03:00:01-07:00"); // 5/6/2018 3:00:01am
  let testFailureDuration = {
    FailureStart: failureStart.utc().format(),
    FailureEnd: failureEnd.utc().format()
  };

  let historyBarProvider = new HistoryBarProvider();
  let errorDivAttributes = historyBarProvider.getErrorDivAttributes(309, now, testFailureDuration);

  expect(errorDivAttributes.left).toEqual(25);
  expect(errorDivAttributes.width).toEqual(1);
});

it('gets a failure duration with no end time', () => {
  let now = moment("2018-05-07T06:00:00-07:00"); // 5/7/2018 6am
  var failureStart = moment("2018-05-07T04:00:00-07:00"); // 5/7/2018 4am
  let testFailureDuration = {
    FailureStart: failureStart.utc().format()
  };

  let historyBarProvider = new HistoryBarProvider();
  let errorDivAttributes = historyBarProvider.getErrorDivAttributes(309, now, testFailureDuration);

  expect(errorDivAttributes.left).toEqual(283);
  expect(errorDivAttributes.width).toEqual(26);
  expect(errorDivAttributes.left + errorDivAttributes.width).toEqual(309);
});

it('gets a failure duration with end time 1 second before now', () => {
  let now = moment("2018-05-07T03:00:00-07:00"); // 5/7/2018 3am
  var failureStart = moment("2018-05-07T02:00:00-07:00"); // 5/7/2018 2am
  var failureEnd = moment("2018-05-07T02:59:59-07:00"); // 5/7/2018 2:59:59am
  let testFailureDuration = {
    FailureStart: failureStart.utc().format(),
    FailureEnd: failureEnd.utc().format()
  };

  let historyBarProvider = new HistoryBarProvider();
  let errorDivAttributes = historyBarProvider.getErrorDivAttributes(309, now, testFailureDuration);

  expect(errorDivAttributes.left).toEqual(296);
  expect(errorDivAttributes.width).toEqual(12);
  expect(errorDivAttributes.width + errorDivAttributes.left).toEqual(308);
});

it('gets a failure duration with a start time before 24 hours ago and end time within 24 hours', () => {
  let now = moment("2018-05-07T01:00:00-07:00"); // 5/7/2018 1am
  var failureStart = moment("2018-05-06T00:30:00-07:00"); // 5/6/2018 12:30am
  var failureEnd = moment("2018-05-06T02:00:00-07:00"); // 5/6/2018 2am
  let testFailureDuration = {
    FailureStart: failureStart.utc().format(),
    FailureEnd: failureEnd.utc().format()
  };

  let historyBarProvider = new HistoryBarProvider();
  let errorDivAttributes = historyBarProvider.getErrorDivAttributes(309, now, testFailureDuration);

  expect(errorDivAttributes.left).toEqual(0);
  expect(errorDivAttributes.width).toEqual(12);
});

it('gets a failure duration with a start time before 24 hours ago and no end time', () => {
  let now = moment("2018-05-07T01:00:00-07:00"); // 5/7/2018 1am
  var failureStart = moment("2018-05-05T01:00:00-07:00"); // 5/5/2018 1am
  let testFailureDuration = {
    FailureStart: failureStart.utc().format()
  };

  let historyBarProvider = new HistoryBarProvider();
  let errorDivAttributes = historyBarProvider.getErrorDivAttributes(309, now, testFailureDuration);

  expect(errorDivAttributes.left).toEqual(0);
  expect(errorDivAttributes.width).toEqual(309);
});

it('gets a failure duration with a start time 1 second before 24 hours ago and end time 1 second within 24 hours', () => {
  let now = moment("2018-05-07T01:00:00-07:00"); // 5/7/2018 1am
  var failureStart = moment("2018-05-06T00:59:59-07:00"); // 5/6/2018 12:59:59am (1 second before 24 hours ago)
  var failureEnd = moment("2018-05-06T01:00:01-07:00"); // 5/6/2018 1:00:01
  let testFailureDuration = {
    FailureStart: failureStart.utc().format(),
    FailureEnd: failureEnd.utc().format()
  };

  let historyBarProvider = new HistoryBarProvider();
  let errorDivAttributes = historyBarProvider.getErrorDivAttributes(309, now, testFailureDuration);

  expect(errorDivAttributes.left).toEqual(0);
  expect(errorDivAttributes.width).toEqual(1);
});

// getDivTitle()
it('get title when failure has a start and end time', () => {
  var failureStart = moment("2018-05-06T03:00:00-07:00"); // 5/6/2018 3am
  var failureEnd = moment("2018-05-06T04:00:00-07:00"); // 5/6/2018 4am
  let testFailureDuration = {
    FailureStart: failureStart.utc().format(),
    FailureEnd: failureEnd.utc().format()
  };

  let historyBarProvider = new HistoryBarProvider();
  let errorDivTitle = historyBarProvider.getDivTitle(testFailureDuration);

  expect(errorDivTitle).toEqual('an hour from 5/6/2018 3:00am - 5/6/2018 4:00am');
});

it('get title when dates have double digits', () => {
  var failureStart = moment("2018-11-12T12:29:34-07:00"); // 11/12/2018 12:29pm
  var failureEnd = moment("2018-11-24T14:40:45-07:00"); // 11/24/2018 2:40pm
  let testFailureDuration = {
    FailureStart: failureStart.utc().format(),
    FailureEnd: failureEnd.utc().format()
  };

  let historyBarProvider = new HistoryBarProvider();
  let errorDivTitle = historyBarProvider.getDivTitle(testFailureDuration);

  expect(errorDivTitle).toEqual('12 days from 11/12/2018 11:29am - 11/24/2018 1:40pm');
});

it('get title when end date is null', () => {
  var failureStart = moment("2018-05-06T03:00:00-07:00"); // 5/6/2018 3am
  let testFailureDuration = {
    FailureStart: failureStart.utc().format(),
    FailureEnd: null
  };

  let historyBarProvider = new HistoryBarProvider();
  let errorDivTitle = historyBarProvider.getDivTitle(testFailureDuration);

  expect(errorDivTitle).toMatch(new RegExp('5/6/2018 3:00am - now'));
});

it('does not throw when dates empty', () => {
  let testFailureDuration = {
    FailureStart: null,
    FailureEnd: null
  };

  let historyBarProvider = new HistoryBarProvider();
  let errorDivTitle = historyBarProvider.getDivTitle(testFailureDuration);

  expect(errorDivTitle).toBeDefined();
  expect(errorDivTitle).toBeTruthy();
});