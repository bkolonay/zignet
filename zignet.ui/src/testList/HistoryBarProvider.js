import React from 'react';
import moment from 'moment';

function getFailureDivs(testFailureDurations, historyBarDiv) {
  const historyBarWidth = historyBarDiv.offsetWidth;
  let now = moment();
  var failureDivs = [];
  for (var i = 0; i < testFailureDurations.length; i++) {
    let failureDivAttributes = getErrorDivAttributes(historyBarWidth, now, testFailureDurations[i]);
    failureDivs.push(<div key={i} style={failureDivAttributes} title={getDivTitle(testFailureDurations[i])}/>);
  }
  return failureDivs;
}

  /* 
    helped with percentage calculations:
     - https://www.calculatorsoup.com/calculators/math/percentage.php
     - https://www.timeanddate.com/date/timeduration.html
  */
  
function getErrorDivAttributes(historyBarWidth, now, testFailureDuration) {
	let oneDayAgo = moment(now.format()).subtract(24, 'hours');
  let testFailureStartTime = moment.utc(testFailureDuration.FailureStart).local();
  let minutesBetweenOneDayAgoAndTestFailureStartTime = moment.duration(testFailureStartTime.diff(oneDayAgo)).asMinutes();

  var failureDivStart = 0;
  if (minutesBetweenOneDayAgoAndTestFailureStartTime > 0) {
    let passingPercentOfLastDay = (minutesBetweenOneDayAgoAndTestFailureStartTime / 1440) * 100;
    failureDivStart = Math.floor((passingPercentOfLastDay / 100) * historyBarWidth);
  }
  else
    testFailureStartTime = oneDayAgo;

  var failureDivWidth = 0;
  if (testFailureDuration.FailureEnd) {
    let testFailureEndTime = moment.utc(testFailureDuration.FailureEnd).local();
    let failureDurationInMinutes = moment.duration(testFailureEndTime.diff(testFailureStartTime)).asMinutes();
    let failingPercentOfLastDay = (failureDurationInMinutes / 1440) * 100;
    failureDivWidth = Math.floor((failingPercentOfLastDay / 100) * historyBarWidth);
    if (failureDivWidth < 1)
    	failureDivWidth = 1;
  }
  else 
    failureDivWidth = historyBarWidth - failureDivStart;

  return {left: failureDivStart, width: failureDivWidth};
}

function getDivTitle(testFailureDuration) {
  let testFailureStartTime = moment.utc(testFailureDuration.FailureStart).local();
  if (testFailureDuration.FailureEnd)
  {
    let testFailureEndTime = moment.utc(testFailureDuration.FailureEnd).local();
    let timeFromStartToEnd = testFailureStartTime.to(testFailureEndTime, true);
    return timeFromStartToEnd + ' from ' + testFailureStartTime.format('l h:mma') + ' - ' + testFailureEndTime.format('l h:mma');
  }
  else
  {
    let timeFromStartToEnd = testFailureStartTime.toNow(true);
    return timeFromStartToEnd + ' from ' + testFailureStartTime.format('l h:mma') + ' - now';
  }      
}

export { getFailureDivs, getErrorDivAttributes, getDivTitle };