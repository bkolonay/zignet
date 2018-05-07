import React, { Component } from 'react';
import TestStatusLabel from './TestStatusLabel'
import TestHistory from './TestHistory'

class SuiteResults extends Component {

  render() {
    const testResult = this.props.testResult;
    return (
  	  <tr>
        <td style={{"width": "55%"}}>{testResult.TestName}</td>
        <td style={{"width": "15%"}}><TestStatusLabel failingFromDate={testResult.FailingFromDate} passingFromDate={testResult.PassingFromDate}/></td>
        <td style={{"width": "30%"}}><TestHistory testFailureDurations={testResult.TestFailureDurations}/></td>
      </tr>
    );
  }
}

export default SuiteResults;
