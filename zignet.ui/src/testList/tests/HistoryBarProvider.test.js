import renderer from 'react-test-renderer';
import moment from 'moment';
import * as HistoryBarProvider from '../HistoryBarProvider'

// getFailureDivs
it('returns zero divs when no failures', () => {
  let historyBarDiv = {offsetWidth: 309}
  const testFailureDurations = [];

  let failureDivs = HistoryBarProvider.getFailureDivs(testFailureDurations, historyBarDiv);
  const tree = renderer.create(failureDivs).toJSON();
  expect(tree).toMatchSnapshot();
});

it('returns 1 div when 1 failure', () => {
  let historyBarDiv = { offsetWidth: 309 }
  let now = moment("2019-04-07T01:00:00-07:00");
  const testFailureDurations = [{
    FailureStart: moment("2019-04-06T03:00:00-07:00").utc().format(),
    FailureEnd: moment("2019-04-06T04:00:00-07:00").utc().format()
  }];

  let failureDivs = HistoryBarProvider.getFailureDivs(testFailureDurations, historyBarDiv, now);
  const tree = renderer.create(failureDivs).toJSON();
  expect(tree).toMatchSnapshot();
});

it('returns 2 divs when 2 failures', () => {
  let historyBarDiv = { offsetWidth: 309 }
  let now = moment("2019-04-07T01:00:00-07:00");
  const testFailureDurations = [{
    FailureStart: moment("2019-04-06T03:00:00-07:00").utc().format(),
    FailureEnd: moment("2019-04-06T04:00:00-07:00").utc().format()
  },
  {
    FailureStart: moment("2019-04-06T04:30:00-07:00").utc().format(),
    FailureEnd: moment("2019-04-06T05:00:00-07:00").utc().format()    
  }];

  let failureDivs = HistoryBarProvider.getFailureDivs(testFailureDurations, historyBarDiv, now);
  const tree = renderer.create(failureDivs).toJSON();
  expect(tree).toMatchSnapshot();
});

// getErrorDivAttributes()
it('gets a failure duration with a start and end time', () => {
  let now = moment("2019-04-07T01:00:00-07:00"); // 5/7/2019 1am
  var failureStart = moment("2019-04-06T03:00:00-07:00"); // 5/6/2019 3am
  var failureEnd = moment("2019-04-06T04:00:00-07:00"); // 5/6/2019 4am
  let testFailureDuration = {
    FailureStart: failureStart.utc().format(),
    FailureEnd: failureEnd.utc().format()
  };

  let errorDivAttributes = HistoryBarProvider.getErrorDivAttributes(309, now, testFailureDuration);

  expect(errorDivAttributes.left).toEqual(25);
  expect(errorDivAttributes.width).toEqual(12);
});

it('gets a failure with a duration of 1 second', () => {
  let now = moment("2019-04-07T01:00:00-07:00"); // 5/7/2019 1am
  var failureStart = moment("2019-04-06T03:00:00-07:00"); // 5/6/2019 3am
  var failureEnd = moment("2019-04-06T03:00:01-07:00"); // 5/6/2019 3:00:01am
  let testFailureDuration = {
    FailureStart: failureStart.utc().format(),
    FailureEnd: failureEnd.utc().format()
  };

  let errorDivAttributes = HistoryBarProvider.getErrorDivAttributes(309, now, testFailureDuration);

  expect(errorDivAttributes.left).toEqual(25);
  expect(errorDivAttributes.width).toEqual(1);
});

it('gets a failure duration with no end time', () => {
  let now = moment("2019-04-07T06:00:00-07:00"); // 5/7/2019 6am
  var failureStart = moment("2019-04-07T04:00:00-07:00"); // 5/7/2019 4am
  let testFailureDuration = {
    FailureStart: failureStart.utc().format()
  };

  let errorDivAttributes = HistoryBarProvider.getErrorDivAttributes(309, now, testFailureDuration);

  expect(errorDivAttributes.left).toEqual(283);
  expect(errorDivAttributes.width).toEqual(26);
  expect(errorDivAttributes.left + errorDivAttributes.width).toEqual(309);
});

it('gets a failure duration with end time 1 second before now', () => {
  let now = moment("2019-04-07T03:00:00-07:00"); // 5/7/2019 3am
  var failureStart = moment("2019-04-07T02:00:00-07:00"); // 5/7/2019 2am
  var failureEnd = moment("2019-04-07T02:59:59-07:00"); // 5/7/2019 2:59:59am
  let testFailureDuration = {
    FailureStart: failureStart.utc().format(),
    FailureEnd: failureEnd.utc().format()
  };

  let errorDivAttributes = HistoryBarProvider.getErrorDivAttributes(309, now, testFailureDuration);

  expect(errorDivAttributes.left).toEqual(296);
  expect(errorDivAttributes.width).toEqual(12);
  expect(errorDivAttributes.width + errorDivAttributes.left).toEqual(308);
});

it('gets a failure duration with a start time before 24 hours ago and end time within 24 hours', () => {
  let now = moment("2019-04-07T01:00:00-07:00"); // 5/7/2019 1am
  var failureStart = moment("2019-04-06T00:30:00-07:00"); // 5/6/2019 12:30am
  var failureEnd = moment("2019-04-06T02:00:00-07:00"); // 5/6/2019 2am
  let testFailureDuration = {
    FailureStart: failureStart.utc().format(),
    FailureEnd: failureEnd.utc().format()
  };

  let errorDivAttributes = HistoryBarProvider.getErrorDivAttributes(309, now, testFailureDuration);

  expect(errorDivAttributes.left).toEqual(0);
  expect(errorDivAttributes.width).toEqual(12);
});

it('gets a failure duration with a start time before 24 hours ago and no end time', () => {
  let now = moment("2019-04-07T01:00:00-07:00"); // 5/7/2019 1am
  var failureStart = moment("2019-04-05T01:00:00-07:00"); // 5/5/2019 1am
  let testFailureDuration = {
    FailureStart: failureStart.utc().format()
  };

  let errorDivAttributes = HistoryBarProvider.getErrorDivAttributes(309, now, testFailureDuration);

  expect(errorDivAttributes.left).toEqual(0);
  expect(errorDivAttributes.width).toEqual(309);
});

it('gets a failure duration with a start time 1 second before 24 hours ago and end time 1 second within 24 hours', () => {
  let now = moment("2019-04-07T01:00:00-07:00"); // 5/7/2019 1am
  var failureStart = moment("2019-04-06T00:59:59-07:00"); // 5/6/2019 12:59:59am (1 second before 24 hours ago)
  var failureEnd = moment("2019-04-06T01:00:01-07:00"); // 5/6/2019 1:00:01
  let testFailureDuration = {
    FailureStart: failureStart.utc().format(),
    FailureEnd: failureEnd.utc().format()
  };

  let errorDivAttributes = HistoryBarProvider.getErrorDivAttributes(309, now, testFailureDuration);

  expect(errorDivAttributes.left).toEqual(0);
  expect(errorDivAttributes.width).toEqual(1);
});

// getDivTitle()
it('get title when failure has a start and end time', () => {
  var failureStart = moment("2019-04-06T03:00:00-07:00"); // 5/6/2019 3am
  var failureEnd = moment("2019-04-06T04:00:00-07:00"); // 5/6/2019 4am
  let testFailureDuration = {
    FailureStart: failureStart.utc().format(),
    FailureEnd: failureEnd.utc().format()
  };

  let errorDivTitle = HistoryBarProvider.getDivTitle(testFailureDuration);

  expect(errorDivTitle).toEqual('an hour from 5/6/2019 3:00am - 5/6/2019 4:00am');
});

it('get title when dates have double digits', () => {
  var failureStart = moment("2019-11-12T12:29:34-07:00"); // 11/12/2019 12:29pm
  var failureEnd = moment("2019-11-24T14:40:45-07:00"); // 11/24/2019 2:40pm
  let testFailureDuration = {
    FailureStart: failureStart.utc().format(),
    FailureEnd: failureEnd.utc().format()
  };

  let errorDivTitle = HistoryBarProvider.getDivTitle(testFailureDuration);

  expect(errorDivTitle).toEqual('12 days from 11/12/2019 11:29am - 11/24/2019 1:40pm');
});

it('get title when end date is null', () => {
  var failureStart = moment("2019-04-06T03:00:00-07:00"); // 5/6/2019 3am
  let testFailureDuration = {
    FailureStart: failureStart.utc().format(),
    FailureEnd: null
  };

  let errorDivTitle = HistoryBarProvider.getDivTitle(testFailureDuration);

  expect(errorDivTitle).toMatch(new RegExp('5/6/2019 3:00am - now'));
});

it('does not throw when dates empty', () => {
  let testFailureDuration = {
    FailureStart: null,
    FailureEnd: null
  };

  let errorDivTitle = HistoryBarProvider.getDivTitle(testFailureDuration);

  expect(errorDivTitle).toBeDefined();
  expect(errorDivTitle).toBeTruthy();
});