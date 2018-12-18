import React, { Component } from 'react';
import TestStatusBadge from './TestStatusBadge'
import TestStatusLabel from './TestStatusLabel'
import TestHistoryBar from './TestHistoryBar'

class TestResultRow extends Component {

  render() {
    const testResult = this.props.testResult;
    return (
  	  <tr>
        <td style={{"width": "52%"}}>{testResult.TestName}</td>
        {null && <td><small className="text-muted">{testResult.SuiteName}</small></td>}
        <td style={{"width": "18%"}}>
          <TestStatusBadge failingFromDate={testResult.FailingFromDate}/>&nbsp;
          <TestStatusLabel failingFromDate={testResult.FailingFromDate} passingFromDate={testResult.PassingFromDate}/>
        </td>
        <td style={{"width": "30%"}}><TestHistoryBar testFailureDurations={testResult.TestFailureDurations}/></td>
      </tr>
    );
  }
}

export default TestResultRow;
