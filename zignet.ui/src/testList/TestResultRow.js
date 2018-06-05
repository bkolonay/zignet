import React, { Component } from 'react';
import TestStatusLabel from './TestStatusLabel'
import TestHistory from './TestHistory'

class SuiteResults extends Component {

  render() {
    const grouped = this.props.grouped;
    const testResult = this.props.testResult;
    return (
  	  <tr>
        <td style={{"width": "52%"}}>{testResult.TestName}</td>
        {grouped && <td><small className="text-muted">{testResult.SuiteName}</small></td>}
        <td style={{"width": "18%"}}><TestStatusLabel failingFromDate={testResult.FailingFromDate} passingFromDate={testResult.PassingFromDate}/></td>
        <td style={{"width": "30%"}}><TestHistory testFailureDurations={testResult.TestFailureDurations}/></td>
      </tr>
    );
  }
}

export default SuiteResults;
