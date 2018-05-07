import moment from 'moment';

it('returns 24 hours ago', () => {
  debugger;

	//let utcNow = moment.utc();
  //let oneDayAgoUtc = moment.utc().subtract(24, 'hours');

  // UTC is hard-coded as 5/6/2018 10AM (end boundary of the bar)
  let utcNow = moment.utc('2018-05-06T10:00:00');
  // 24 hours (1440 minutes) before this is 5/5/2018 10AM (start boundary of the bar)
  let oneDayAgoUtc = moment.utc(utcNow.format()).subtract(24, 'hours');

  // the time the test started failing (22 hours ago, or 2 hours after the start of the bar boundary)
  let testFailureStartTime = moment.utc('2018-05-05T12:00:00');
  // get number of minutes between 24 hours ago and when the test started failing (120 minutes)
  // _this is the amount of time the test was PASSING before it started failing_
  let minutesBetweenOneDayAgoAndTestFailureTime = moment.duration(testFailureStartTime.diff(oneDayAgoUtc)).asMinutes();
  // get the percentage of time the test was PASSING (in minutes) within the last 24 hours (8.3332%)
  let passingPercentOfLastDay = (minutesBetweenOneDayAgoAndTestFailureTime / 1440) * 100;

  // convert this to a percentage of the bar pixel width (25.647 pixels)
  // _this is the OFFSET of where the failure div should START_
  // TODO: account/test for when div should start at 0
  let failureDivStart = (passingPercentOfLastDay / 100) * 309;

  // the time the test stopped failing (21 hours ago, or 3 hours after the start of the bar boundary)
  let testFailureEndTime = moment.utc('2018-05-05T13:00:00');
  // get the number of minutes between the failure start and end (60 minutes)
  // TODO: account for when there is no end time
  let failureDurationInMinutes = moment.duration(testFailureEndTime.diff(testFailureStartTime)).asMinutes();

  // get the percent of time the test was failing within the past 24 hours (4.17%)
	let failingPercentOfLastDay = (failureDurationInMinutes / 1440) * 100;

	// convert this to a percentage of the bar pixel width (12.8853)
	// _this is the WIDTH or the failure div_
	let failureDivWidth = (failingPercentOfLastDay / 100) * 309;

  let stop = "stop";

  // TODO: is it OK to ignore failures less than a minute? should everything be converted to seconds? (do we want to show seconds of downtime: yes, probably)

  // https://www.calculatorsoup.com/calculators/math/percentage.php
  // https://www.timeanddate.com/date/timeduration.html
});