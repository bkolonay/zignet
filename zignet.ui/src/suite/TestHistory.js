import React, { Component } from 'react';
import moment from 'moment';
import './testHistory.css';

class TestHistory extends Component {
  constructor() {
    super();
    this.state = {
      failureDivs: []
    };
  }  

  componentDidMount() {
    let historyBarWidth = this.historyBarDiv.offsetWidth

    let now = moment();
    let oneDayAgo = moment(now.format()).subtract(24, 'hours');

    var failureDivs = [];
    var testFailureDurations = this.props.testFailureDurations;
    for (var i = 0; i < testFailureDurations.length; i++) {
      // the time the test started failing (e.g. 22 hours ago, or 2 hours after the start of the bar boundary)
      let testFailureStartTime = moment.utc(testFailureDurations[i].FailureStart).local();
      // get number of minutes between 24 hours ago and when the test started failing (e.g. 120 minutes)
      // _this is the amount of time the test was PASSING before it started failing_
      let minutesBetweenOneDayAgoAndTestFailureTime = moment.duration(testFailureStartTime.diff(oneDayAgo)).asMinutes();
      // get the percentage of time the test was PASSING (in minutes) within the last 24 hours (e.g. 8.3332%)
      let passingPercentOfLastDay = (minutesBetweenOneDayAgoAndTestFailureTime / 1440) * 100;

      // convert this to a percentage of the bar pixel width (e.g. 25.647 pixels)
      // _this is the OFFSET of where the failure div should START_
      // TODO: account/test for when div should start at 0
      let failureDivStart = (passingPercentOfLastDay / 100) * historyBarWidth;

      // the time the test stopped failing (e.g. 21 hours ago, or 3 hours after the start of the bar boundary)
      let testFailureEndTime = moment.utc(testFailureDurations[i].FailureEnd).local();
      // get the number of minutes between the failure start and end (e.g. 60 minutes)
      // TODO: account for when there is no end time
      let failureDurationInMinutes = moment.duration(testFailureEndTime.diff(testFailureStartTime)).asMinutes();

      // get the percent of time the test was failing within the past 24 hours (e.g. 4.17%)
      let failingPercentOfLastDay = (failureDurationInMinutes / 1440) * 100;

      // convert this to a percentage of the bar pixel width (e.g. 12.8853)
      // _this is the WIDTH or the failure div_
      let failureDivWidth = (failingPercentOfLastDay / 100) * historyBarWidth;

      failureDivs.push(<div key={i} style={{left: failureDivStart, width: failureDivWidth}}/>);
    }
    this.setState({
      failureDivs: failureDivs
    });
  }

  render() {
    return (
      <div
        className="testHistoryBar"
        ref={(div) => this.historyBarDiv = div}
      >
      {this.state.failureDivs.map((failureDiv) => failureDiv)}
      
      </div>
    );
  }
}

export default TestHistory;

// {this.props.testFailureDurations.length}
//width: {this.historyBarWidth}
      //<div style={{left: 25, width: 12}}/>