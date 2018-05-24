import React, { Component } from 'react';
import TestStatusLabel from './TestStatusLabel'
import TestHistory from './TestHistory'

class SuiteResults extends Component {

  _getSuiteNameRow(testResult, testResultsGrouped) {
    if (testResultsGrouped)
      return <td><small className="text-muted">{testResult.SuiteName}</small></td>
  }

  render() {
    const testResult = this.props.testResult;
    return (
  	  <tr>
        <td style={{"width": "52%"}}>{testResult.TestName}</td>
        {this._getSuiteNameRow(testResult, this.props.testResultsGrouped)}
        <td style={{"width": "18%"}}><TestStatusLabel failingFromDate={testResult.FailingFromDate} passingFromDate={testResult.PassingFromDate}/></td>
        <td style={{"width": "30%"}}><TestHistory testFailureDurations={testResult.TestFailureDurations}/></td>
      </tr>
    );
  }
}

export default SuiteResults;
